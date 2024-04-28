using Microsoft.Extensions.Configuration;
using PixelPilot.Models;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World;
using PixelPilot.PixelGameClient.World.Constants;
using PixelPilotExample;

// Load the configuration. Don't store your account token in the code :)
var config = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .Build().Get<BasicConfig>();

if (config == null)
{
    Console.WriteLine("The configuration file could not be loaded.");
    return;
}

// Create a client.
var client = new PixelPilotClient(config.AccountToken, false);

// Create a PixelWorld class and attach the client to it.
// Allow it to listen to client updates.
var world = new PixelWorld();
client.OnPacketReceived += world.HandlePacket;
world.OnBlockPlaced += (_, playerId, oldBlock, newBlock) =>
{
    if (client.BotId == playerId) return;
    //client.Send(new PlayerChatOutPacket($"A {newBlock.Block} was placed by user with ID 1!"));
    client.Send(oldBlock.AsPacketOut());
};

await client.Connect(RoomType.Pixelwalker3, "r082b210d67df52");


// Executed when the client receives a packet!
client.OnPacketReceived += (_, packet) =>
{
    // Use strongly typed packets!
    if (packet is PlayerChatPacket chat)
    {
        if (chat.Message.Equals(".stop"))
        {
            client.Disconnect();
            Environment.Exit(0);
            return;
        }
        if (chat.Message.Equals(".ping")) client.Send(new PlayerChatOutPacket($"Pong! ({chat.Id})"));
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

// Don't terminate.
Thread.Sleep(-1);