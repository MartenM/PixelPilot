using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Queue;

/// <summary>
/// Rate limits outgoing messages by consuming tokens.
/// </summary>
public class TokenBucketPacketOutQueue : IPixelPacketQueue
{
    private readonly ILogger _logger = LogManager.GetLogger("PacketOutQueue");

    private int _totalRateLimit = OwnerTotalRateLimit;
    private int _chatRateLimit = OwnerChatRateLimit;

    public const int OwnerTotalRateLimit = 250;
    public const int OwnerChatRateLimit = 15;

    public const int GuestTotalRateLimit = 100;
    public const int GuestChatRateLimit = 15;
    
    /// <summary>
    /// Maximum amount of packets stored up.
    /// </summary>
    private const int MaxBurst = 50;
    
    /// <summary>
    /// Tokens replenished per second
    /// </summary>
    private const int BurstTotal = 20;

    private CancellationTokenSource _cancellationToken;
    private Task? _processingTask;
    public bool IsProcessing { get; private set; }

    private PixelPilotClient _client;

    private TokenBucketRateLimiter _totalRateLimiter;

    private TokenBucketRateLimiter _chatRateLimiter;

    private int _chatReplenishTime;
    
    private bool _isOwner = true;

    public bool IsOwner
    {
        get => _isOwner;
        set
        {
            if (_isOwner == value) return;
            _isOwner = value;

            if (_isOwner)
            {
                _totalRateLimit = GuestTotalRateLimit;
                _chatRateLimit = GuestChatRateLimit;
            }
            else
            {
                _totalRateLimit = OwnerTotalRateLimit;
                _chatRateLimit = OwnerChatRateLimit;
            }
            
            // Set property, recreate the rate limiters.
            _logger.LogDebug("Recreating rate limiters. Limits have been changed.");
            
            CreateRateLimiters();
        }
    }
    
    /// <summary>
    /// Initial queue
    /// </summary>
    private readonly BlockingCollection<IMessage> _packetQueue = new();
    
    /// <summary>
    /// Queue for chat messages that got delayed.
    /// Should be send as soon as space becomes available again.
    /// </summary>
    private readonly Queue<IMessage> _delayedChatMessageQueue = new();

    public TokenBucketPacketOutQueue(PixelPilotClient client)
    {
        _client = client;
        _cancellationToken = new CancellationTokenSource();
        IsProcessing = false;
        
        _totalRateLimiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions()
        {
            QueueLimit = 1,
            TokenLimit = MaxBurst,
            ReplenishmentPeriod = TimeSpan.FromMilliseconds((1000.00 /  _totalRateLimit) * BurstTotal),
            TokensPerPeriod = BurstTotal
        });
        
        CreateRateLimiters();
    }

    /// <summary>
    /// Creates the rate limiters based on the properties.
    /// </summary>
    private void CreateRateLimiters()
    {
        _totalRateLimiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions()
        {
            QueueLimit = 1,
            TokenLimit = MaxBurst,
            ReplenishmentPeriod = TimeSpan.FromMilliseconds((1000D /  _totalRateLimit) * BurstTotal),
            TokensPerPeriod = BurstTotal
        });
        
        _chatRateLimiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions()
        {
            QueueLimit = 1,
            TokenLimit = 1,
            ReplenishmentPeriod = TimeSpan.FromMilliseconds(1000D / _chatRateLimit),
            TokensPerPeriod = 1
        });
        
        var totalTime = (int) _totalRateLimiter.ReplenishmentPeriod.TotalMilliseconds;
        _chatReplenishTime = (int) _chatRateLimiter.ReplenishmentPeriod.TotalMilliseconds;
        
        _logger.LogDebug($"Total replenish duration {totalTime}");
        _logger.LogDebug($"Chat replenish duration {_chatReplenishTime}");
        if (totalTime < 15)
        {
            _logger.LogWarning("Queue replenish time is lower then 15ms. The queue will still work, but it can't go faster like this.");
        }
    }

    public void EnqueuePacket(IMessage packet)
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
            if (packet is PlayerChatPacket chatPacket)
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