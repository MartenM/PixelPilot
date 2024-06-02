using Example.BasicBot;
using Example.CommandBot.Commands;
using Example.CommandBot.Commands.SubCommand;
using Microsoft.Extensions.Configuration;
using PixelPilot.ChatCommands;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.Players.Basic;

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

// A custom command manager that implements our permissions.
// This is not required, but very useful!
var commandManager = new CustomCommandManager(client, playerManager);
commandManager.AddHelpCommand();
commandManager.AddCommand(new BotActionsRoot(client));
commandManager.AddCommand(new TestCommand());

// Executed once the client receives INIT
// Make a platform and do some silly loops.
client.OnClientConnected += (_) =>
{
    client.SendChat("Hello world using the PixelPilot API.");
};

// Connect to a room.
await client.Connect("mknckr7oqxq24xa");

// Don't terminate.
Thread.Sleep(-1);