using PixelPilot.Client;
using PixelPilot.Client.Players;
using PixelPilot.Client.Players.Basic;

namespace PixelPilot.ChatCommands;

public class CommandManager : PixelChatCommandManager<Player>
{
    public CommandManager(PixelPilotClient client, PixelPlayerManager<Player> pixelPlayerManager) : base(client, pixelPlayerManager)
    {
        
    }
}