using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Players;
using PixelPilot.PixelGameClient.Players.Basic;

namespace PixelPilot.ChatCommands;

public class CommandManager : PixelChatCommandManager<Player>
{
    public CommandManager(PixelPilotClient client, PixelPlayerManager<Player> pixelPlayerManager) : base(client, pixelPlayerManager)
    {
        
    }
}