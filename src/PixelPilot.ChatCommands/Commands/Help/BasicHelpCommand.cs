namespace PixelPilot.ChatCommands.Commands.Help;

public class BasicHelpCommand : ChatCommand
{
    private IChatCommandManager _chatCommandManager;
    
    public BasicHelpCommand(IChatCommandManager chatCommandManager) : base("help", "Show all commands.", null)
    {
        _chatCommandManager = chatCommandManager;
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        var cmds = _chatCommandManager.GetAvailableCommands(sender);
        _chatCommandManager.HelpFormatter.SendHelp(sender, cmds);
        return Task.CompletedTask;
    }
}