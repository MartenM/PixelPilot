namespace PixelPilot.ChatCommands.Commands.Help;

public interface IHelpFormatter
{
    void SendHelp(ICommandSender sender, List<ChatCommand> subCommands);
}