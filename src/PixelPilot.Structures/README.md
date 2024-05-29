# PixelPilot.Structures
Structures are used to load and save blocks. This is an optional package.

## 📄 Documentation
- [https://martenm.github.io/PixelPilotDocs](https://martenm.github.io/PixelPilotDocs/guides/introduction.html)

### JSON Format
This is an example. This example has been shortened to fit here so it might not actually be a valid and working save file.
```json
{
  // Version of this save
  "Version": 1,
  // Width & Height of this structure
  "Width": 1,
  "Height": 1,
  // Meta tags to be defined by the user
  "Meta": {
    "key": "value"
  },
  // If this structure has saved empty blocks
  "ContainsEmpty": false,
  "Blocks": {
    // Mapping of blocks. ID's in blockdata are replaced by temporary IDs.
    // BricksGrass: 0, Coin: 1
    "Mapping": [
      "BricksGrass",
      "Coin"
    ],
    // Blockdata as found in the world buffer in INIT.
    "BlockData": [
      "AQAAAAsAAAABAAAAAAAAAA==",
      "AgAAAAgAAAABAAAAAQAAAA=="
    ]
  }
}
```


## Example bot:
```csharp
using System.Drawing;
using System.Text.Json;
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
            client.Disconnect();
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
            world.GetDifference(structure, (int) player.X / 16,  (int) player.Y / 16).PasteShuffled(client, new Point((int)(player.X / 16), (int)(player.Y / 16)), 5);
            
            break;
        }
        case PlayerChatPacket { Message: ".diff" } chat:
        {
            string json = File.ReadAllText("test-struct.json");
            var structure = PilotSaveSerializer.Deserialize(json);

            if (structure == null) return;
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
```