using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class PortalBlock : BasicBlock
{
    public int PortalId { get; set; }
    public int TargetId { get; set; }
    
    public int Direction { get; set; }
    
    public PortalBlock(int x, int y, int layer, int blockId, int portalId, int targetId, int direction) : base(x, y, layer, blockId)
    {
        PortalId = portalId;
        TargetId = targetId;
        Direction = direction;
    }

    public override IPixelGamePacketOut AsPacketOut()
    {
        return new WorldBlockPlacedOutPacket(X, Y, Layer, BlockId, Direction, PortalId, TargetId, null);
    }
}