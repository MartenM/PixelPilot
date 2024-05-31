using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using PixelPilot.Common;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Exceptions;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelHttpClient;
using Websocket.Client;

namespace PixelPilot.PixelGameClient;

/// <summary>
/// Client for interacting with the PixelWalker game server.
/// </summary>
public class PixelPilotClient : IDisposable
{
    // Constants
    public static readonly int SecondsBeforeGatewayTimeout = 3;
    
    private readonly ILogger _logger = LogManager.GetLogger("Client");
    public readonly PixelApiClient ApiClient;
    private WebsocketClient? _socketClient;
    private PacketConverter _packetConverter = new();
    public string? RoomType { get; private set; }

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
    /// The player ID of the client.
    /// </summary>
    public int? BotId { get; private set; }
    
    /// <summary>
    /// Event that occurs when a packet is received.
    /// </summary>
    public event PacketReceived? OnPacketReceived;
    public delegate void PacketReceived(object sender, IPixelGamePacket packet);
    
    /// <summary>
    /// Event that occurs when a packet is send.
    /// </summary>
    public event PacketSend? OnPacketSend;
    public delegate void PacketSend(object sender, IPixelGamePacketOut packet);
    
    /// <summary>
    /// Fired once init has been received by the client.
    /// </summary>
    public event ClientConnected? OnClientConnected;
    public delegate void ClientConnected(object sender);

    /// <summary>
    /// Create a PixelPilot client using an account token.
    /// Automatically reconnects.
    /// </summary>
    /// <param name="accountToken">A valid account token</param>
    public PixelPilotClient(string accountToken) : this(accountToken, true)
    { }
    
    /// <summary>
    /// Create a PixelPilot client using an account token.
    /// </summary>
    /// <param name="accountToken">A valid account token</param>
    /// <param name="automaticReconnect">If the bot should reconnect</param>
    public PixelPilotClient(string accountToken, bool automaticReconnect)
    {
        ApiClient = new PixelApiClient(accountToken);
        AutomaticReconnect = automaticReconnect;
    }
    
    /// <summary>
    /// Create a PixelPilotClient using the email and password login method.
    /// Automatically reconnects.
    /// </summary>
    /// <param name="email">Valid email</param>
    /// <param name="password">Valid password</param>
    public PixelPilotClient(string email, string password) : this(email, password, true)
    { }
    
    /// <summary>
    /// Create a PixelPilotClient using the email and password login method.
    /// </summary>
    /// <param name="email">Valid email</param>
    /// <param name="password">Valid password</param>
    /// <param name="automaticReconnect">If the bot should reconnect</param>
    public PixelPilotClient(string email, string password, bool automaticReconnect)
    {
        ApiClient = new PixelApiClient(email, password);
        AutomaticReconnect = automaticReconnect;
    }

    /// <summary>
    /// Connects to a game room using the specified room type and room ID.
    /// </summary>
    /// <param name="roomType">The type of the room.</param>
    /// <param name="roomId">The ID of the room.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Connect(string roomId)
    {
        var roomType = (await ApiClient.GetRoomTypes())![0];
        
        var joinRequest = await ApiClient.GetJoinKey(roomType, roomId);
        if (joinRequest?.Token == null)
        {
            throw new Exception("Failed to get the join key. Are you sure that the account token is still valid?");
        }
        
        _logger.LogInformation("Successfully acquired the room token. Connecting now.");
        var gameRoomUrl = $"{EndPoints.GameWebsocketEndpoint}/room/{joinRequest.Token}";
        
        _socketClient = new WebsocketClient(new Uri(gameRoomUrl));
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
            _logger.LogWarning($"Client got disconnected. ({info.CloseStatusDescription})");
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
                _logger.LogError(ex, "Something went wrong while processing a socket message.");
            }
        });

        // Start the websocket.
        await _socketClient.Start();
    }

    /// <summary>
    /// Disconnects the socket client gracefully by stopping it with a normal closure status.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task Disconnect()
    {
        _socketClient?.Stop(WebSocketCloseStatus.NormalClosure, "Socket closed by the client.");
        IsConnected = false;
        return Task.CompletedTask;
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
    /// Sends a pixel game packet.
    /// </summary>
    /// <param name="packet">The pixel game packet to send.</param>
    public void Send(IPixelGamePacketOut packet)
    {
        OnPacketSend?.Invoke(this, packet);
        _send(packet.ToBinaryPacket());
    }
    
    /// <summary>
    /// Sends a chat message while ensuring that the message doesn't become too long.
    /// </summary>
    /// <param name="msg">The message</param>
    public void SendChat(string msg)
    {
        var maxLineLength = 120;
        var charCount = 0;
        
        var lines = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(w => (charCount += w.Length + 1) / maxLineLength)
            .Select(g => string.Join(" ", g));

        foreach (var line in lines)
        {
            Send(new PlayerChatOutPacket(line));
        }
    }

    /// <summary>
    /// Same as SendChat but as PM.
    /// </summary>
    /// <param name="username">Player username</param>
    /// <param name="msg">The message</param>
    public void SendPm(string username, string msg)
    {
        var maxLineLength = 100;
        var charCount = 0;
        
        var lines = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .GroupBy(w => (charCount += w.Length + 1) / maxLineLength)
            .Select(g => string.Join(" ", g));

        foreach (var line in lines)
        {
            Send(new PlayerChatOutPacket($"/pm {username} {line}"));
        }
    }
    
    /// <summary>
    /// Handles the received response message, parsing it into a pixel game packet and triggering relevant events.
    /// </summary>
    /// <param name="message">The response message to handle.</param>
    private void OnMessageReceived(ResponseMessage message)
    {
        if (message.Binary == null) return;
        
        IPixelGamePacket packet;
        try
        {
            packet = PacketConverter.ConstructPacket(message.Binary);
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
        if (packet is PingPacket)
        {
            _send(new byte[] {0x3F});
            return;
        }

        // Init packet needs to return the first 2 bytes.
        if (packet is InitPacket init)
        {
            _send(InitPacket.AsSendingBytes());

            _logger.LogInformation("Connected to the room successfully");
            IsConnected = true;
            BotId = init.PlayerId;
            
            InvokeWithTimings("ClientConnected", () =>
            {
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
                _logger.LogWarning($"A {name} handler is blocking the GateWay task. This might result in your bot disconnecting! Consider offloading heavy-work to a different thread.");
            }

            await handlerTask.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"A {name} handler has thrown an unexpected error.");
        }
    }
    
    public void Dispose()
    {
        ApiClient.Dispose();
        _socketClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}