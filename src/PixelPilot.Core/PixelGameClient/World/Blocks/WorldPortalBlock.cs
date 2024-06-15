using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class WorldPortalBlock : BasicBlock
{
    public string WorldId { get; set; }

    public WorldPortalBlock(string worldId) : base(PixelBlock.WorldPortal)
    {
        WorldId = worldId;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [WorldId]);
    }
}