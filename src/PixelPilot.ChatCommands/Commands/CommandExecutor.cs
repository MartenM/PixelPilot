namespace PixelPilot.ChatCommands.Commands;

public interface ICommandExecutor
{
    public Task ExecuteCommand(CommandSender sender, string fullCommand, string[] args);
}