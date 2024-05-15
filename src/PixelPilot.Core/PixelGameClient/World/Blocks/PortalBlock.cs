using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class PortalBlock : BasicBlock
{
    public int PortalId { get; set; }
    public int TargetId { get; set; }
    
    public int Direction { get; set; }
    
    public PortalBlock(int blockId, int portalId, int targetId, int direction) : base(blockId)
    {
        PortalId = portalId;
        TargetId = targetId;
        Direction = direction;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, Direction, PortalId, TargetId, null);
    }
}