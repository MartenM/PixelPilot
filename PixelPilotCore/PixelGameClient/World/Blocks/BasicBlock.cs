using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Constants;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Basic blocks only have a block ID.
/// No special data can be assigned to them.
/// </summary>
public class BasicBlock : IPixelBlock
{
    public BasicBlock(int x, int y, int layer, int blockId)
    {
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Layer { get; set; }
    public int BlockId { get; set; }
    
    public virtual IPixelGamePacketOut AsPacketOut()
    {
        return new WorldBlockPlacedOutPacket(X, Y, Layer, BlockId);
    }
}