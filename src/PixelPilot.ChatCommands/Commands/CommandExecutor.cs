namespace PixelPilot.ChatCommands.Commands;

public interface ICommandExecutor
{
    public Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args);
}