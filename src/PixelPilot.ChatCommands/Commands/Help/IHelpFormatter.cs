namespace PixelPilot.ChatCommands.Commands.Help;

public interface IHelpFormatter
{
    void SendHelp(CommandSender sender, List<ChatCommand> subCommands);
}