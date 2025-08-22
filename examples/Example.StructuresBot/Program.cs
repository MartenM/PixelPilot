// See https://aka.ms/new-console-template for more information

// Load the configuration. Don't store your account token in the code :)
using System.Drawing;
using Example.BasicBot;
using Microsoft.Extensions.Configuration;
using PixelPilot.Client;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.Players;
using PixelPilot.Client.Players.Basic;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks.Types;
using PixelPilot.Client.World.Constants;
using PixelPilot.Common.Logging;
using PixelPilot.Structures;
using PixelPilot.Structures.Converters.PilotSimple;
using PixelPilot.Structures.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddJsonFile("config.development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

LogManager.Configure(configuration.GetSection("Logging"));
var config = configuration.Get<BasicConfig>();
if (config == null)
{
    Console.WriteLine("The configuration file could not be loaded.");
    return;
}

// Basic things and vars
Structure? currentStructure = null;
var point1 = new Point(0, 0);
var point2 = new Point(0, 0);

var client = PixelPilotClient.Builder()
    // .SetToken(config.AccountToken)
    .SetEmail(config.AccountEmail)
    .SetPassword(config.AccountPassword)
    .SetPrefix("[StructBot] ")
    .SetAutomaticReconnect(false)
    .Build();

var world = new PixelWorld(client);
client.OnPacketReceived += world.HandlePacket;
world.OnWorldInit += sender =>
{
    point2 = new Point(world.Width - 1, world.Height - 1);
};

var playerManager = new PlayerManager();
client.OnPacketReceived += playerManager.HandlePacket;

world.OnBlocksPlaced += async (sender, blocksEvent) =>
{
    if (client.BotId == blocksEvent.UserId) return;

    if (blocksEvent.NewBlock.Block.ToString().Contains("Red"))
    {
        // No red blocks allowed.
        client.SendChat("You shall not place red blocks!");
        blocksEvent.Cancelled = true;
    }
};

// Setup some basic commands. Only allow me to execute them.
client.OnPacketReceived += (_, packet) =>
{
    var playerId = packet.GetPlayerId();
    if (playerId == null) return;
    
    IPixelPlayer? player = playerManager.GetPlayer(playerId.Value);
    if (player == null) return;

    

    // Simple command structures.
    if (packet is PlayerChatPacket chat)
    {
        if (!chat.Message.StartsWith(".") || player.Username != "MARTEN") return;

        var fullText = chat.Message.Substring(1);
        var args = fullText.Split(' ');

        switch (args[0])
        {
            case "exec":
                client.SendChat($"/{string.Join(" ", args.Skip(1))}", prefix: false);
                break;
            case "p1":
                point1 = new Point(player.BlockX, player.BlockY);
                client.SendChat($"Point 1 has been set. {point1}");
                break;
            case "p2":
                point2 = new Point(player.BlockX, player.BlockY);
                client.SendChat($"Point 2 has been set. {point2}");
                break;
            case "select":
            case "copy":
                currentStructure = world.GetStructure(point1, point2, false);
                client.SendChat("Current structure has been set.");
                break;
            case "save":
                if (currentStructure == null)
                {
                    client.SendChat("Please copy a structure using .copy");
                    return;
                }
                
                if (args.Length < 2 || currentStructure == null)
                {
                    client.SendChat("Please provide a file name.");
                    return;
                }
                
                var rawSave = PilotSaveSerializer.Serialize(currentStructure);
                File.WriteAllText($"./{args[1]}.json", rawSave);
                
                client.SendChat("Structure saved to file.");
                break;
            case "load":
                if (args.Length < 2)
                {
                    client.SendChat("Please provide a file name.");
                    return;
                }

                var rawLoad = File.ReadAllText($"./{args[1]}.json");
                currentStructure = PilotSaveSerializer.Deserialize(rawLoad);
                
                client.SendChat("Structure loaded from file.");
                break;
            case "paste":
            {
                if (currentStructure == null) return;
                
                var pasteX = player.BlockX;
                var pasteY = player.BlockY;
                
                // Get the difference in packets. Then chunk the result together and send the packets.
                // world.GetDifference(currentStructure, pasteX, pasteY).PasteInOrder(client, new Point(0, 0));

                var packets = world.GetDifference(currentStructure, pasteX, pasteY).ToChunkedPackets();

                if (packets.Count == 0)
                {
                    client.SendChat("Nothing to paste. All blocks already there!");
                }
                else
                {
                    client.SendChat($"Pasting structure... {packets.Count}");
                    foreach(var bp in packets)
                    {
                        if (bp is WorldBlockPlacedPacket blockPlaced)
                        {
                            if (blockPlaced.BlockId == (int) PixelBlock.FallLeavesYellowBg)
                            {
                                Console.WriteLine($"Layer {(WorldLayer) blockPlaced.Layer} PLACING: {string.Join(",", blockPlaced.Positions.Select(p => $"({p.X} {p.Y})"))}");
                                //client.SendChat($"PLACING THEM {blockPlaced.Positions}");
                            }
                        }
                        client.Send(bp);
                    }
                }
                
                break;
            }
        }
        return;
    }
};

await client.Connect("8l04w8nv2ey451v");
await world.InitTask;

client.SendChat("Connected!");
await client.WaitForDisconnect();