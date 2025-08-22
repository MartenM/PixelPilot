using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using PixelPilot.Api;
using PixelPilot.Client.Abstract;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Exceptions;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.Messages.Queue;
using PixelPilot.Client.Players;
using PixelPilot.Common;
using PixelPilot.Common.Logging;
using PixelWalker.Networking.Protobuf.WorldPackets;
using Websocket.Client;

namespace PixelPilot.Client;

/// <summary>
/// Client for interacting with the PixelWalker game server.
/// </summary>
public class PixelPilotClient : IPixelPilotClient, IDisposable
{
    // Constants
    private static readonly int SecondsBeforeGatewayTimeout = 3;
    private const int ChatCharLimit = 150;
    
    private readonly TaskCompletionSource<bool> _connectCompletion = new();
    
    private readonly ILogger _logger = LogManager.GetLogger("Client");
    public readonly PixelApiClient ApiClient;
    private WebsocketClient? _socketClient;
    
    public string? BotPrefix { get; set; } = null;
    private IPixelPacketQueue? _packetOutQueue;

    public bool DisposeApi { get; set; } = true;
    
    public Exception? LastException { get; private set; }

    /// <summary>
    /// Indicates if the client will try to automatically reconnect if the
    /// connection gets somehow lost.
    /// </summary>
    public bool AutomaticReconnect
    {
        get;
        private set;
    }
    
    /// <summary>
    /// Indicates if the client is connected.
    /// </summary>
    public bool IsConnected
    {
        get;
        private set;
    }
    
    /// <summary>
    /// The PlayerProperties of the bot account.
    /// </summary>
    public PlayerProperties? BotPlayerProperties { get; private set; }

    /// <summary>
    /// The ID used for the bot in the world.
    /// </summary>
    public int? BotId => BotPlayerProperties?.PlayerId;

    /// <summary>
    /// The username of this bot.
    /// </summary>
    public string? Username => BotPlayerProperties?.Username;
    
    /// <summary>
    /// Event that occurs when a packet is received.
    /// </summary>
    public event PacketReceived? OnPacketReceived;
    public delegate void PacketReceived(object sender, IMessage packet);
    
    /// <summary>
    /// Event that occurs when a packet is send. The packet is the oneof WorldPacket (so not the WorldPacket).
    /// </summary>
    public event PacketSend? OnPacketSend;
    public delegate void PacketSend(object sender, IMessage packet);
    
    /// <summary>
    /// Fired once init has been received by the client.
    /// </summary>
    public event ClientConnected? OnClientConnected;
    public delegate void ClientConnected(object sender);
    
    /// <summary>
    /// Fired when the client has been disconnected.
    /// Optional reason is available when present on the socket connection.
    /// </summary>
    public event ClientDisconnected? OnClientDisconnected;
    public delegate void ClientDisconnected(object sender, string? reason);
    
    public PixelPilotClient(PixelApiClient apiClient, bool automaticReconnect, string? botPrefix, Func<PixelPilotClient, IPixelPacketQueue?> configurePacketQueue)
    {
        ApiClient = apiClient;
        AutomaticReconnect = automaticReconnect;
        BotPrefix = botPrefix;

        _packetOutQueue = configurePacketQueue.Invoke(this);
    }

    public static PixelGameClientBuilder Builder() => new();

    /// <summary>
    /// Connects to a game room using the specified room type and room ID.
    /// </summary>
    /// <param name="roomId">The ID of the room.</param>
    /// <param name="joinData">Join data, required for creating unsaved worlds.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Connect(string roomId, JoinData? joinData = null)
    {
        var roomType = (await ApiClient.GetRoomTypes())![0];
        
        var joinRequest = await ApiClient.GetJoinKey(roomType, roomId);
        if (joinRequest?.Token == null)
        {
            throw new Exception("Failed to get the join key. Are you sure that the account token is still valid?");
        }
        
        _logger.LogInformation("Successfully acquired the room token");
        var gameRoomUrl = $"{EndPoints.GameWebsocketEndpoint}/ws?joinkey={joinRequest.Token}";

        if (joinData != null)
        {
            var binaryJoinData = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(joinData, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }));
            var connectUrl = new Uri($"{gameRoomUrl}&joinData={Convert.ToBase64String(binaryJoinData)}");
            _socketClient = new WebsocketClient(connectUrl);
        }
        else
        {
            _socketClient = new WebsocketClient(new Uri(gameRoomUrl));
        }
        
        _socketClient.ReconnectTimeout = TimeSpan.FromSeconds(5000);
        _socketClient.IsReconnectionEnabled = AutomaticReconnect;
        _socketClient.ReconnectionHappened.Subscribe(info =>
        {
            if (info.Type == ReconnectionType.Initial) return;
            _logger.LogWarning($"Reconnection happened, type: {info.Type}");
        });
        _socketClient.DisconnectionHappened.Subscribe(info =>
        {
            IsConnected = false;
            _packetOutQueue?.Stop();
            _logger.LogWarning($"Websocket was closed. ({info.CloseStatusDescription} {(Username != null ? $"Bot: {Username}": null)})");
            _connectCompletion.TrySetResult(true);
            OnClientDisconnected?.Invoke(this, info.CloseStatusDescription);
        });
        _socketClient.MessageReceived.Subscribe(msg =>
        {
            try
            {
                OnMessageReceived(msg);
            }
            catch (Exception ex)
            {
                // Sometimes things go wrong. Don't crash the socket in that case.
                _logger.LogError(ex, $"Something went wrong while processing a socket message. {(Username != null ? $"(Bot: {Username})": null)}");
                LastException = ex;
            }
        });

        // Start the websocket. Set connected to True.
        
        await _socketClient.Start();

        var timeout = Task.Delay(TimeSpan.FromSeconds(5));
        var completedTask = await Task.WhenAny(_connectCompletion.Task, timeout);
        
        if (completedTask == _connectCompletion.Task && IsConnected)
        {
            _logger.LogInformation("Connected to the room successfully");
        }
        else
        {
            _logger.LogWarning("Could not join the room.");
        }
    }

    /// <summary>
    /// Disconnects the socket client gracefully by stopping it with a normal closure status.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Disconnect()
    {
        if (_packetOutQueue != null) await _packetOutQueue.Stop();
        if (_socketClient?.IsRunning ?? false) _socketClient?.Stop(WebSocketCloseStatus.NormalClosure, "Socket closed by the client.");
        IsConnected = false;
    }
    
    /// <summary>
    /// Sends a byte array through the socket client if it is connected.
    /// </summary>
    /// <param name="raw">The byte array to send.</param>
    private void _send(byte[] raw)
    {
        if (_socketClient == null)
        {
            _logger.LogWarning("Attempting to send a message but the client hasn't connected yet!");
            return;
        }
        
        _logger.LogDebug($"Sending: {BitConverter.ToString(raw).Replace("-", " ")}");
        _socketClient.Send(raw);
    }
    
    /// <summary>
    /// Send the packet. Wraps it into a WorldPacket and converts it to binary and calls _Send(byte[] raw);
    /// </summary>
    /// <param name="packet">The packet to send.</param>
    private void _send(IMessage packet)
    {
        _send(packet.ToWorldPacket().ToByteArray());
    }
    
    /// <summary>
    /// Sends a pixel game packet. Wraps it into a WorldPacket.
    /// </summary>
    /// <param name="packet">The pixel game packet to send.</param>
    public void SendDirect(IMessage packet)
    {
        OnPacketSend?.Invoke(this, packet);
        _send(packet);
    }

    /// <summary>
    /// Sends a pixel game packet.
    /// Uses an internal rate limiter to limit packets.
    /// </summary>
    /// <param name="packet">The pixel game packet to send.</param>
    public void Send(IMessage packet)
    {
        if (_packetOutQueue == null) SendDirect(packet);
        else _packetOutQueue?.EnqueuePacket(packet);
    }

    /// <summary>
    /// Sends multiple pixel game packets.
    /// Uses an internal rate limiter to limit packets.
    /// </summary>
    /// <param name="packets">The packets to be send</param>
    public void SendRange(IEnumerable<IMessage> packets)
    {
        foreach (var packet in packets)
        {
            if (_packetOutQueue == null) SendDirect(packet);
            else _packetOutQueue?.EnqueuePacket(packet);
        }
    }

    /// <summary>
    /// Sends a chat message while ensuring that the message doesn't become too long.
    /// </summary>
    /// <param name="msg">The message</param>
    /// <param name="prefix">If the message should be prefixed</param>
    public void SendChat(string msg, bool prefix = true)
    {
        var maxLineLength = ChatCharLimit - (prefix && BotPrefix != null ? BotPrefix.Length : 0);
        var charCount = 0;

        var lines = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(w => (charCount += w.Length + 1) / maxLineLength)
            .Select(g => string.Join(" ", g));

        foreach (var line in lines)
        {
            Send(new PlayerChatPacket()
            {
                Message = $"{(prefix && BotPrefix != null ? BotPrefix : "")}{line}"
            });
        }
    }

    /// <summary>
    /// Same as SendChat but as PM.
    /// </summary>
    /// <param name="player">Target player</param>
    /// <param name="msg">The message</param>
    /// <param name="prefix">If the message should contain the prefix</param>
    public void SendPm(IPixelPlayer player, string msg, bool prefix = false)
    {
        SendPm(player.Username, msg, prefix);
    }

    /// <summary>
    /// Same as SendChat but as PM.
    /// </summary>
    /// <param name="username">Player username</param>
    /// <param name="msg">The message</param>
    /// <param name="prefix">If the message should contain the prefix</param>
    public void SendPm(string username, string msg, bool prefix = true)
    {
        var maxLineLength = 100 - (prefix ? (BotPrefix?.Length ?? 0) : 0);
        var charCount = 0;
        
        var lines = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(w => (charCount += w.Length + 1) / maxLineLength)
            .Select(g => string.Join(" ", g));

        foreach (var line in lines)
        {
            Send(new PlayerChatPacket()
            {
                Message = $"/pm {username} {(prefix && BotPrefix != null ? BotPrefix : "")}{line}"
            });
        }
    }
    
    /// <summary>
    /// Handles the received response message, parsing it into a pixel game packet and triggering relevant events.
    /// </summary>
    /// <param name="message">The response message to handle.</param>
    private void OnMessageReceived(ResponseMessage message)
    {
        if (message.Binary == null) return;
        
        IMessage packet;
        try
        {
            var worldPacket = WorldPacket.Parser.ParseFrom(message.Binary);
            packet = worldPacket.GetPacket();
        }
        catch (PixelException)
        {
            // Either not implemented or something else. Logging is done in the packet converter.
            // We caught it so we are aware of it's existence. That's enough.
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something horribly wrong while deconstructing the packet. Please report this error.");
            return;
        }

        _logger.LogDebug($"Received packet: {packet.GetType()}");

        // Ping packet needs to be returned.
        // Doesn't require any additional handling.
        if (packet is Ping)
        {
            _send(new Ping());
            return;
        }

        // Handle init and store some properties.
        // Sends back the PlayerInitReceivedPacket.
        if (packet is PlayerInitPacket init)
        {
            _send(new PlayerInitReceivedPacket());

            BotPlayerProperties = init.PlayerProperties;
            
            // Update rate limit if required
            if (_packetOutQueue != null)
            {
                var isWorldOwner = init.PlayerProperties.IsWorldOwner;
                _packetOutQueue.IsOwner = isWorldOwner;

                if (!isWorldOwner)
                {
                    _logger.LogInformation("You are not the world owner. Rate limiting is being applied.");
                }
            }
            
            // We connected!
            IsConnected = true;
            _connectCompletion.TrySetResult(true);
            
            InvokeWithTimings("ClientConnected", () =>
            {
                _packetOutQueue?.Start();
                return Task.Run(() => OnClientConnected?.Invoke(this));
            }).Wait();
        }

        // Fire the event to be used by the API users.
        InvokeWithTimings("PacketReceived", () =>
        {
            return Task.Run(() => OnPacketReceived?.Invoke(this, packet));
        }).Wait();
    }

    private async Task InvokeWithTimings(string name, Func<Task> invoker)
    {
        try
        {
            var timerTask = Task.Delay(TimeSpan.FromSeconds(SecondsBeforeGatewayTimeout));
            var handlerTask = invoker();
            
            // Run both, check which one finishes first.
            if (await Task.WhenAny(timerTask, handlerTask).ConfigureAwait(false) == timerTask)
            {
                _logger.LogWarning($"A {name} handler is blocking the GateWay task. This might result in your bot disconnecting! Consider offloading heavy-work to a different thread. {(Username != null ? $"(Bot: {Username})": null)}");
            }

            await handlerTask.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"A {name} handler has thrown an unexpected error.");
        }
    }

    public int PacketQueueSize => _packetOutQueue?.QueueSize ?? 0;

    public Task WaitForEmptyQueue(int checkTime = 1000)
    {
        return Task.Run(async () =>
        {
            while (PacketQueueSize > 0)
                await Task.Delay(checkTime);
        });
    }

    public async Task WaitForDisconnect(CancellationToken ct = new())
    {
        await Task.Run(async () =>
        {
            while (IsConnected)
            {
                await Task.Delay(1000);
            }
        }, ct).ConfigureAwait(true);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _packetOutQueue?.Dispose();
        if(DisposeApi) ApiClient.Dispose();
        _socketClient?.Dispose();
    }
}