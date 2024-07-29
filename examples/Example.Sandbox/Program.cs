using System.Drawing;
using Example.BasicBot;
using Microsoft.Extensions.Configuration;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient;
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
    .Build();

// Player manager allows you to easily keep track of player stats.
// For advanced users, it can be extended to include relevant information for you.
var playerManager = new PlayerManager();
client.OnPacketReceived += playerManager.HandlePacket;

// Create a PixelWorld class and attach the client to it.
// Allow it to listen to client updates. Not required!
var world = new PixelWorld();
client.OnPacketReceived += world.HandlePacket;
world.OnBlockPlaced += (_, playerId, oldBlock, block) =>
{
    if (playerId == client.BotId) return;
    
    client.Send(oldBlock.AsPacketOut());
    Thread.Sleep(100);
    client.Send(block.AsPacketOut());
};


// Executed when the client receives a packet!
var p1 = new Point(0, 0);
var p2 = new Point(99, 99);
client.OnPacketReceived += async (_, packet) =>
{
    
};

// Executed once the client receives INIT
// Make a platform and do some silly loops.
client.OnClientConnected += (_) =>
{
    client.Send(new PlayerChatOutPacket("Hello world using the PixelPilot API."));
};

// Connect to a room.
await client.Connect("ozkju7mpvrofgwd");
client.Send(new PlayerChatOutPacket("I'm alive!"));


// Don't terminate.
await client.WaitForDisconnect();