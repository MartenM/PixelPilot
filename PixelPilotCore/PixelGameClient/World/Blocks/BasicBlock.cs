using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Blocks.Placed;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Basic blocks only have a block ID.
/// No special data can be assigned to them.
/// </summary>
public class BasicBlock : IPixelBlock
{
    public BasicBlock(int blockId)
    {
        BlockId = blockId;
    }
    
    public int BlockId { get; set; }
    
    public virtual IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x,y, layer, BlockId);
    }

    public IPlacedBlock AsPlacedBlock(int x, int y, int layer)
    {
        return new PlacedBlock(x, y, layer, this);
    }
}