using PixelPilot.Client.Messages.Received;

namespace PixelPilot.Client.Players.Basic;

/// <summary>
/// Basic implementation of the player manager.
/// </summary>
public class PlayerManager : PixelPlayerManager<Player>
{
    protected override Player CreatePlayer(PlayerJoinPacket join)
    {
        return new Player(join);
    }
}