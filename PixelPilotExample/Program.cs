using Microsoft.Extensions.Configuration;
using PixelPilot;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World;
using PixelPilotExample;

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
var client = new PixelPilotClient(config.AccountToken, false);

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
    switch (packet)
    {
        // Use strongly typed packets!
        case PlayerChatPacket { Message: ".stop" }:
            client.Disconnect();
            Environment.Exit(0);
            return;
        case PlayerChatPacket { Message: ".ping" } chat:
        {
            client.Send(new PlayerChatOutPacket($"Pong! ({chat.PlayerId})"));
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

// Connect to a room.
await client.Connect("r082b210d67df52");

// Don't terminate.
Thread.Sleep(-1);