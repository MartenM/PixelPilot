using Example.BasicBot;
using Example.CommandBot.Commands;
using Example.CommandBot.Commands.SubCommand;
using Microsoft.Extensions.Configuration;
using PixelPilot.ChatCommands;
using PixelPilot.Client;
using PixelPilot.Client.Players.Basic;
using PixelPilot.Common.Logging;

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
await client.Connect("r082b210d67df52");

// Don't terminate.
await client.WaitForDisconnect();
Console.WriteLine("End of program reached.");