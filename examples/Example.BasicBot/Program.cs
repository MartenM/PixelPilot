using Example.BasicBot;
using Microsoft.Extensions.Configuration;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.Players.Basic;
using PixelPilot.PixelGameClient.World;

// Load the configuration. Don't store your account token in the code :)
var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .Build();

LogManager.Configure(configuration.GetSection("Logging"));
var config = configuration.Get<BasicConfig>();
if (config == null)
{
    Console.WriteLine("The configuration file could not be loaded.");
    return;
}

// Create a client.
var client = PixelPilotClient.Builder()
    .SetToken(config.AccountToken)
    .SetAutomaticReconnect(false)
    .SetPrefix("[Bot] ")
    .Build();

// Player manager allows you to easily keep track of player stats.
// For advanced users, it can be extended to include relevant information for you.
var playerManager = new PlayerManager();
client.OnPacketReceived += playerManager.HandlePacket;

// Create a PixelWorld class and attach the client to it.
// Allow it to listen to client updates. Not required!
var world = new PixelWorld();
client.OnPacketReceived += world.HandlePacket;
world.OnBlockPlaced += (_, playerId, oldBlock, _) =>
{
    if (client.BotId == playerId) return;
    client.Send(oldBlock.AsPacketOut());
};


// Executed when the client receives a packet!
client.OnPacketReceived += (_, packet) =>
{
    // Make use of strongly typed packets!
    switch (packet)
    {
        case PlayerChatPacket { Message: ".stop" }:
            client.Disconnect().Wait();
            return;
        case PlayerChatPacket { Message: ".ping" } chat:
        {
            var player = playerManager.GetPlayer(chat.PlayerId);
            if (player == null) return;

            client.Send(new PlayerChatOutPacket($"Pong! ({player.Username}, {player.X}, {player.Y})"));
            break;
        }
        case PlayerJoinPacket join:
            client.Send(new PlayerChatOutPacket($"/giveedit {join.Username}"));
            break;
    }
};

// Executed once the client receives INIT
// Make a platform and do some silly loops.
client.OnClientConnected += (_) =>
{
    client.Send(new PlayerChatOutPacket("Hello world using the PixelPilot API."));
    Thread.Sleep(250);
    PlatformUtil.GetThread(client).Start();
    client.Send(new PlayerMoveOutPacket(592, 1056, 0, 0, 0, 0, 0, 0, false, false, 100)); 
};

client.OnClientDisconnected += (_, reason) =>
{
    Console.WriteLine($"Disconnected with reason: {reason}");
};

// Connect to a room.
await client.Connect("r082b210d67df52");

// Don't terminate.
await client.WaitForDisconnect();