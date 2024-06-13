using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Queue;

/// <summary>
/// Rate limits outgoing messages by consuming tokens.
/// </summary>
public class TokenBucketPacketOutQueue : IPixelPacketQueue, IDisposable
{
    private readonly ILogger _logger = LogManager.GetLogger("PacketOutQueue");
    
    private const int TotalRateLimit = 250;
    private const int ChatRateLimit = 15;

    private const int MaxBurst = 50;
    private const int BurstTotal = 20;

    private CancellationTokenSource _cancellationToken;
    private Task? _processingTask;
    public bool IsProcessing { get; private set; }

    private PixelPilotClient _client;

    private readonly TokenBucketRateLimiter _totalRateLimiter = new(new TokenBucketRateLimiterOptions()
    {
        QueueLimit = 1,
        TokenLimit = MaxBurst,
        ReplenishmentPeriod = TimeSpan.FromMilliseconds((1000D /  TotalRateLimit) * BurstTotal),
        TokensPerPeriod = BurstTotal
    });
    
    private readonly TokenBucketRateLimiter _chatRateLimiter = new(new TokenBucketRateLimiterOptions()
    {
        QueueLimit = 1,
        TokenLimit = 1,
        ReplenishmentPeriod = TimeSpan.FromMilliseconds(1000D /  ChatRateLimit),
        TokensPerPeriod = 1
    });

    private int _chatReplenishTime;
    
    /// <summary>
    /// Initial queue
    /// </summary>
    private readonly BlockingCollection<IPixelGamePacketOut> _packetQueue = new();
    
    /// <summary>
    /// Queue for chat messages that got delayed.
    /// Should be send as soon as space becomes available again.
    /// </summary>
    private readonly Queue<PlayerChatOutPacket> _delayedChatMessageQueue = new();

    public TokenBucketPacketOutQueue(PixelPilotClient client)
    {
        _client = client;
        _cancellationToken = new CancellationTokenSource();
        IsProcessing = false;

        var totalTime = (int) _totalRateLimiter.ReplenishmentPeriod.TotalMilliseconds;
        _chatReplenishTime = (int) _chatRateLimiter.ReplenishmentPeriod.TotalMilliseconds;
        
        _logger.LogDebug($"Total replenish duration {totalTime}");
        _logger.LogDebug($"Chat replenish duration {_chatReplenishTime}");
        if (totalTime < 15)
        {
            _logger.LogWarning("Queue replenish time is lower then 15ms. The queue will still work, but it can't go faster like this.");
        }
    }

    public void EnqueuePacket(IPixelGamePacketOut packet)
    {
        _packetQueue.Add(packet);
    }

    public int QueueSize => _packetQueue.Count + _delayedChatMessageQueue.Count;

    public Task Start()
    {
        if (IsProcessing) return Task.CompletedTask;

        IsProcessing = true;
        _processingTask = Task.Run(async () =>
        {
            try
            {
                await ProcessQueue();
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("TokenBucketPacketOutQueue has Task has been cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured.");
            } 
        });
        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        if (!IsProcessing) return;
        await _cancellationToken.CancelAsync();
        await _processingTask?.WaitAsync(TimeSpan.FromSeconds(5))!;
        IsProcessing = false;
    }

    private async Task ProcessQueue()
    {
        // We only assume one client is working on this at the time.
        while (!_cancellationToken.Token.IsCancellationRequested)
        {
            // Handle delayed packets.
            if (_delayedChatMessageQueue.Count > 0 && _chatRateLimiter.AttemptAcquire().IsAcquired)
            {
                await _totalRateLimiter.AcquireAsync(1, _cancellationToken.Token);
                _client.SendDirect(_delayedChatMessageQueue.Dequeue());
                continue;
            }
            
            // Handle main queue
            _packetQueue.TryTake(out var packet, _chatReplenishTime, _cancellationToken.Token);
            if (packet == null) continue;
            
            // Send the packet. Delay the treat yay.
            if (packet is PlayerChatOutPacket chatPacket)
            {
                // Check if chat rate limit is fine.
                // Otherwise schedule for later.
                if (!_chatRateLimiter.AttemptAcquire().IsAcquired)
                {
                    // TODO: Switch out with delayed packet if available.
                    _delayedChatMessageQueue.Enqueue(chatPacket);
                    continue;
                }
            }

            await _totalRateLimiter.AcquireAsync(1, _cancellationToken.Token);
            _client.SendDirect(packet);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _cancellationToken.Dispose();
        _totalRateLimiter.Dispose();
        _chatRateLimiter.Dispose();
        _packetQueue.Dispose();
    }
}