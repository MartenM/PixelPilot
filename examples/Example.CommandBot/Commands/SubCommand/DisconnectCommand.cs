using PixelPilot.ChatCommands;
using PixelPilot.ChatCommands.Commands;
using PixelPilot.Client;

namespace Example.CommandBot.Commands.SubCommand;

public class DisconnectCommand : ChatCommand
{
    private PixelPilotClient _client;
    
    public DisconnectCommand(PixelPilotClient client) : base("disconnect", "Disconnect the bot", "+disconnect")
    {
        _client = client;
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        sender.SendMessage("Goodbye");
        _client.Disconnect();
        return Task.CompletedTask;
    }
}