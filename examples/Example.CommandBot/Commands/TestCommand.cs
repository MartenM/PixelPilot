using PixelPilot.ChatCommands;
using PixelPilot.ChatCommands.Commands;

namespace Example.CommandBot.Commands;

public class TestCommand : ChatCommand
{
    public TestCommand() : base("test", "A test command", null)
    {
        
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        sender.SendMessage("This is a test command!");
        return Task.CompletedTask;
    }
}