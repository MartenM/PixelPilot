using PixelPilot.ChatCommands.Commands;
using PixelPilot.ChatCommands.Commands.Help;

namespace PixelPilot.ChatCommands;

public interface IChatCommandManager
{
    public IHelpFormatter HelpFormatter { get; }
    public List<ChatCommand> GetAvailableCommands(CommandSender sender);
}