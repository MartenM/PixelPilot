using System.Drawing;
using Example.BasicBot;
using Microsoft.Extensions.Configuration;
using PixelPilot.Client;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.Players;
using PixelPilot.Client.Players.Basic;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Constants;
using PixelPilot.Common.Logging;
using PixelPilot.Structures.Converters.PilotSimple;
using PixelPilot.Structures.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

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
world.OnBlockPlaced += (_, playerId, oldBlock, newBlock) =>
{
    if (playerId == client.BotId) return;
    
    Console.WriteLine(newBlock.Block.Block);
};


// Executed when the client receives a packet!
client.OnPacketReceived += (_, packet) =>
{
	var playerId = packet.GetPlayerId();
	if (playerId == null) return;

	IPixelPlayer? player = playerManager.GetPlayer(playerId.Value);
	if (player == null) return;
	switch (packet)
	{
		case PlayerChatPacket { Message: "place"}:
            client.Send(new PlacedBlock(0, 0, WorldLayer.Foreground, new BasicBlock(PixelBlock.HazardSpikesBrownUp)).AsPacketOut());
            break;
	}
};

// Executed once the client receives INIT
// Make a platform and do some silly loops.
client.OnClientConnected += (_) =>
{
    client.SendChat("Hello world using the PixelPilot API.");
};

// Connect to a room.
await client.Connect($"pixelpilot_testing", new JoinData()
{
    WorldHeight = 400,
    WorldWidth = 636,
    WorldTitle = "[Sandbox]"
});
client.SendChat("I'm alive!");


// Don't terminate.
await client.WaitForDisconnect();