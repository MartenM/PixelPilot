using System.Threading.Channels;
using System.Threading.RateLimiting;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Queue;

public class TokenBucketPacketOutQueue : IPixelPacketQueue
{
    private readonly ILogger _logger = LogManager.GetLogger("Client.Messages.Queue");

    public const int OwnerTotalRateLimit = 250;
    public const int OwnerChatRateLimit = 15;

    public const int GuestTotalRateLimit = 100;
    public const int GuestChatRateLimit = 5;

    private const int MaxBurst = 50;
    private const int BurstTotal = 20;

    private readonly PixelPilotClient _client;

    private readonly Channel<IMessage> _channel;
    private readonly Channel<IMessage> _delayedChatChannel;

    private CancellationTokenSource _cts = new();
    private Task? _processingTask;

    private TokenBucketRateLimiter _totalLimiter = null!;
    private TokenBucketRateLimiter _chatLimiter = null!;

    private int _totalRateLimit = OwnerTotalRateLimit;
    private int _chatRateLimit = OwnerChatRateLimit;
    
    private int _mainQueueCount;
    private int _delayedQueueCount;

    private bool _isOwner = true;

    public bool IsProcessing { get; private set; }

    public bool IsOwner
    {
        get => _isOwner;
        set
        {
            if (_isOwner == value) return;
            _isOwner = value;

            _totalRateLimit = _isOwner ? OwnerTotalRateLimit : GuestTotalRateLimit;
            _chatRateLimit = _isOwner ? OwnerChatRateLimit : GuestChatRateLimit;

            _logger.LogInformation("Recreating rate limiters. IsOwner: {IsOwner}", _isOwner);
            RecreateLimiters();
        }
    }

    public int QueueSize => _mainQueueCount + _delayedQueueCount;

    public TokenBucketPacketOutQueue(PixelPilotClient client)
    {
        _client = client;

        _channel = Channel.CreateUnbounded<IMessage>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        _delayedChatChannel = Channel.CreateUnbounded<IMessage>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        RecreateLimiters();
    }

    private void RecreateLimiters()
    {
        var totalReplenish = TimeSpan.FromMilliseconds((1000d / _totalRateLimit) * BurstTotal);
        var chatReplenish = TimeSpan.FromMilliseconds(1000d / _chatRateLimit);

        var newTotal = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
        {
            TokenLimit = MaxBurst,
            TokensPerPeriod = BurstTotal,
            ReplenishmentPeriod = totalReplenish,
            QueueLimit = int.MaxValue
        });

        var newChat = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
        {
            TokenLimit = 1,
            TokensPerPeriod = 1,
            ReplenishmentPeriod = chatReplenish,
            QueueLimit = int.MaxValue
        });

        var oldTotal = Interlocked.Exchange(ref _totalLimiter, newTotal);
        var oldChat = Interlocked.Exchange(ref _chatLimiter, newChat);

        oldTotal?.Dispose();
        oldChat?.Dispose();

        _logger.LogDebug("Total replenish {Ms}ms", totalReplenish.TotalMilliseconds);
        _logger.LogDebug("Chat replenish {Ms}ms", chatReplenish.TotalMilliseconds);
    }

    public void EnqueuePacket(IMessage packet)
    {
        if (_channel.Writer.TryWrite(packet))
        {
            Interlocked.Increment(ref _mainQueueCount);
        }
    }

    public Task Start()
    {
        if (IsProcessing) return Task.CompletedTask;

        IsProcessing = true;
        _cts = new CancellationTokenSource();

        _processingTask = Task.Run(ProcessAsync);
        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        if (!IsProcessing) return;

        _cts.Cancel();

        if (_processingTask != null)
        {
            await _processingTask.WaitAsync(TimeSpan.FromSeconds(5));
        }

        IsProcessing = false;
    }

    private async Task ProcessAsync()
    {
        var token = _cts.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                // Prioritize delayed chat
                if (_delayedChatChannel.Reader.TryRead(out var delayed))
                {
                    Interlocked.Decrement(ref _delayedQueueCount);
                    
                    await _chatLimiter.AcquireAsync(1, token);
                    await _totalLimiter.AcquireAsync(1, token);

                    _client.SendDirect(delayed);
                    continue;
                }

                var packet = await _channel.Reader.ReadAsync(token);
                Interlocked.Decrement(ref _mainQueueCount);

                if (packet is PlayerChatPacket)
                {
                    var lease = await _chatLimiter.AcquireAsync(1, token);
                    if (!lease.IsAcquired)
                    {
                        await _delayedChatChannel.Writer.WriteAsync(packet, token);
                        Interlocked.Increment(ref _delayedQueueCount);
                        continue;
                    }
                }

                await _totalLimiter.AcquireAsync(1, token);
                _client.SendDirect(packet);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("Processing cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Processing error");
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        _totalLimiter.Dispose();
        _chatLimiter.Dispose();
    }
}
