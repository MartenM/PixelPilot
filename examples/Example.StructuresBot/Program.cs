using System.Drawing;
using Microsoft.Extensions.Configuration;
using Example.BasicBot;
using PixelPilot.Client;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.Players;
using PixelPilot.Client.Players.Basic;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;
using PixelPilot.Common.Logging;
using PixelPilot.Structures;
using PixelPilot.Structures.Converters;
using PixelPilot.Structures.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

#region Configuration

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddJsonFile("config.development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

LogManager.Configure(configuration.GetSection("Logging"));

var config = configuration.Get<BasicConfig>();

if (config == null)
{
    Console.WriteLine("Failed to load configuration.");
    return;
}

#endregion

#region Runtime State

var state = new BotState();

#endregion

#region Client Setup

var client = PixelPilotClient.Builder()
    .SetEmail(config.AccountEmail)
    .SetPassword(config.AccountPassword)
    .SetPrefix("[StructBot] ")
    .SetAutomaticReconnect(false)
    .Build();

var world = new PixelWorld(client);
client.OnPacketReceived += world.HandlePacket;

var playerManager = new PlayerManager();
client.OnPacketReceived += playerManager.HandlePacket;

#endregion

#region World Init

world.OnWorldInit += _ =>
{
    state.Point2 = new Point(world.Width - 1, world.Height - 1);
};

#endregion

#region Block Constants
const PixelBlock Point1SelectorBlock = PixelBlock.CoinGold;
const PixelBlock Point2SelectorBlock = PixelBlock.CoinBlue;
#endregion

#region Block Placement Handling

world.OnBlocksPlaced += async (_, blocksEvent) =>
{
    if (!state.Enabled)
        return;

    var placerId = blocksEvent.UserId;

    // Ignore bot placements
    if (placerId == client.BotId)
        return;

    var player = playerManager.GetPlayer(placerId);

    if (player == null)
        return;

    //
    // Protected blocks example
    //
    if (blocksEvent.NewBlock.Block == PixelBlock.CrownGold)
    {
        blocksEvent.Cancelled = true;
        return;
    }

    //
    // Selection blocks
    //
    if (blocksEvent.Positions.Count() == 1)
    {
        var blockPos = blocksEvent.Positions.First();
        if (blocksEvent.NewBlock.Block == Point1SelectorBlock)
        {
            state.Point1 = new Point(
                blockPos.X,
                blockPos.Y
            );

            blocksEvent.Cancelled = true;
            client.SendChat($"Point 1 set to {state.Point1}");
            return;
        }

        if (blocksEvent.NewBlock.Block == Point2SelectorBlock)
        {
            state.Point2 = new Point(
                blockPos.X,
                blockPos.Y
            );

            blocksEvent.Cancelled = true;
            client.SendChat($"Point 2 set to {state.Point2}");
            return;
        }
        
        //
        // Waiting for paste origin
        //
        if (state.WaitingForPasteOrigin)
        {
            state.WaitingForPasteOrigin = false;
            blocksEvent.Cancelled = true;

            if (state.CurrentStructure == null)
            {
                client.SendChat("No structure loaded.");
                return;
            }

            var pasteX = blockPos.X;
            var pasteY = blockPos.Y;

            await PasteStructure(
                client,
                world,
                state.CurrentStructure,
                pasteX,
                pasteY
            );

            client.SendChat($"Structure pasted at {pasteX}, {pasteY}");
        }
    }
};

#endregion

#region Chat Commands

client.OnPacketReceived += (_, packet) =>
{
    if (packet is not PlayerChatPacket chat)
        return;

    var playerId = packet.GetPlayerId();

    if (playerId == null)
        return;

    var player = playerManager.GetPlayer(playerId.Value);

    if (player == null)
        return;

    // Restrict commands
    if (player.Username != client.Username)
        return;

    if (!chat.Message.StartsWith("."))
        return;

    var args = chat.Message[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

    if (args.Length == 0)
        return;

    var command = args[0].ToLowerInvariant();

    var playerPosition = new Point(
        (int)Math.Round(player.X / 16.0),
        (int)Math.Round(player.Y / 16.0)
    );

    switch (command)
    {
        case "on":
        {
            state.Enabled = true;
            client.SendChat("Bot enabled.");
            break;
        }

        case "off":
        {
            state.Enabled = false;
            state.WaitingForPasteOrigin = false;
            client.SendChat("Bot disabled.");
            break;
        }

        case "p1":
        {
            state.Point1 = playerPosition;
            client.SendChat($"Point 1 set to {state.Point1}");
            break;
        }

        case "p2":
        {
            state.Point2 = playerPosition;
            client.SendChat($"Point 2 set to {state.Point2}");
            break;
        }

        case "copy":
        case "select":
        {
            state.CurrentStructure = world.GetStructure(
                state.Point1,
                state.Point2,
                false
            );

            client.SendChat("Structure copied.");
            break;
        }
        case "world":
        {
            state.CurrentStructure = world.GetWorldStructure(copyEmpty: false);
            client.SendChat("World Structure copied.");
            break;
        }

        case "save":
        {
            if (state.CurrentStructure == null)
            {
                client.SendChat("No structure copied.");
                return;
            }

            if (args.Length < 2)
            {
                client.SendChat("Provide a filename.");
                return;
            }

            var fileName = $"{args[1]}.json";

            var raw = PilotSaveSerializer.Serialize(state.CurrentStructure);

            File.WriteAllText(fileName, raw);

            client.SendChat($"Saved to {fileName}");
            break;
        }

        case "load":
        {
            if (args.Length < 2)
            {
                client.SendChat("Provide a filename.");
                return;
            }

            var fileName = $"{args[1]}.json";

            if (!File.Exists(fileName))
            {
                client.SendChat("File does not exist.");
                return;
            }

            var raw = File.ReadAllText(fileName);

            state.CurrentStructure = PilotSaveSerializer.Deserialize(raw);

            if (state.CurrentStructure.Meta.ContainsKey(WorldStructure.WorldSettingsKey))
            {
                state.CurrentStructure = new WorldStructure(state.CurrentStructure);
                client.SendChat("World Structure loaded.");
            }
            else
            {
                client.SendChat("Structure loaded.");
            }

           
            break;
        }

        case "paste":
        {
            if (state.CurrentStructure == null)
            {
                client.SendChat("No structure loaded.");
                return;
            }

            state.WaitingForPasteOrigin = true;

            client.SendChat("Place a block to choose paste origin.");
            break;
        }

        case "cancel":
        {
            state.WaitingForPasteOrigin = false;
            client.SendChat("Cancelled pending actions.");
            break;
        }
    }
};

#endregion

#region Connection

await client.Connect("r91a2a75381ca22");

client.SendChat("Connected!");

await client.WaitForDisconnect();

#endregion

#region Helpers

async Task PasteStructure(
    PixelPilotClient client,
    PixelWorld world,
    Structure structure,
    int x,
    int y)
{
    var difference = world.GetDifference(structure, x, y);

    var packets = difference
        .AsPackets()
        .ToList();
    
    if (structure is WorldStructure worldStructure)
    {
        var settings = worldStructure.WorldSettings;
        if (settings != null)
        {
            client.SendChat("Also applying world settings...");
            client.Send(settings.AsUpdatePacket());
        }
    }

    if (packets.Count == 0)
    {
        client.SendChat("Nothing to paste.");
        return;
    }

    client.SendChat($"Pasting {packets.Count} packets...");

    foreach (var packet in packets)
    {
        client.Send(packet);

        // Optional throttle
        await Task.Delay(5);
    }
}

#endregion

#region State

class BotState
{
    public bool Enabled { get; set; } = true;

    public bool WaitingForPasteOrigin { get; set; }

    public Point Point1 { get; set; } = new(0, 0);

    public Point Point2 { get; set; } = new(0, 0);

    public Structure? CurrentStructure { get; set; }
}

#endregion