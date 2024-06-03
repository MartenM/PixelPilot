using System.Drawing;
using System.Text.Json;
using Example.BasicBot;
using Microsoft.Extensions.Configuration;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.Players.Basic;
using PixelPilot.PixelGameClient.World;
using PixelPilot.Structures.Converters.PilotSimple;
using PixelPilot.Structures.Extensions;

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
world.OnBlockPlaced += (_, playerId, oldBlock, _) =>
{
    
};


// Executed when the client receives a packet!
var p1 = new Point(8, 8);
var p2 = new Point(16, 19);
client.OnPacketReceived += async (_, packet) =>
{
    var playerPacket = packet as IPixelGamePlayerPacket;
    if (playerPacket == null) return;

    var player = playerManager.GetPlayer(playerPacket.PlayerId);
    if (player == null) return;
    
    // Make use of strongly typed packets!
    switch (packet)
    {
        case PlayerChatPacket { Message: ".stop" }:
            await client.Disconnect();
            Environment.Exit(0);
            return;
        case PlayerChatPacket { Message: ".test" }:
            playerManager.Players
                .Where(p => p.Deaths > 2)
                .ToList()
                .ForEach(p =>
                {
                    Task.Run(async () =>
                    {
                        client.Send(new PlayerChatOutPacket($"/pm {p.Username} Man you should die less often!"));
                        await Task.Delay(1000);
                        client.Send(new PlayerChatOutPacket($"/reset {p.Username}"));
                        client.Send(new PlayerChatOutPacket($"/pm {p.Username} Or maybe..."));
                        await Task.Delay(1000);
                        client.Send(new PlayerChatOutPacket($"/kick {p.Username}"));
                    });
                });
            return;
        case PlayerChatPacket { Message: ".p1" } chat:
        {
            p1 = new Point((int)(player.X / 16), (int)(player.Y / 16));
            client.Send(new PlayerChatOutPacket(p1.ToString()));
            break;
        }
        case PlayerChatPacket { Message: ".p2" } chat:
        {
            p2 = new Point((int)(player.X / 16), (int)(player.Y / 16));
            client.Send(new PlayerChatOutPacket(p2.ToString()));
            break;
        }
        case PlayerChatPacket { Message: ".save" } chat:
        {
            var structure = world.GetStructure(p1, p2, copyEmpty: false);
            
            // Save
            var json = PilotSaveSerializer.Serialize(structure);
            File.WriteAllText("test-struct.json", json);
            client.Send(new PlayerChatOutPacket("Struct saved!"));
            break;
        }
        case PlayerChatPacket { Message: ".load" } chat:
        {
            string json = File.ReadAllText("test-struct.json");
            var structure = PilotSaveSerializer.Deserialize(json);
            
            client.Send(new PlayerChatOutPacket("Struct pasting..."));
            for (int i = 0; i < 100; i++) client.Send(new PlayerChatOutPacket("Chat message... " + i));
            await world.GetDifference(structure, (int) player.X / 16,  (int) player.Y / 16).PasteInOrder(client, new Point((int)(player.X / 16), (int)(player.Y / 16)));
            
            break;
        }
        case PlayerChatPacket { Message: ".diff" } chat:
        {
            string json = File.ReadAllText("test-struct.json");
            var structure = PilotSaveSerializer.Deserialize(json);

            client.Send(new PlayerChatOutPacket("Struct pasting..."));
            world.GetDifference(structure, (int) player.X / 16,  (int) player.Y / 16).ForEach(b => Console.WriteLine(JsonSerializer.Serialize(b)));
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
};

// Connect to a room.
await client.Connect("mknckr7oqxq24xa");

// Don't terminate.
Thread.Sleep(-1);