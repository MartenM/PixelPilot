
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Players.Basic;

/// <summary>
/// Basic implementation of the player manager.
/// </summary>
public class PlayerManager : PixelPlayerManager<Player>
{
    protected override Player CreatePlayer(PlayerJoinedPacket join)
    {
        return new Player(join);
    }
}