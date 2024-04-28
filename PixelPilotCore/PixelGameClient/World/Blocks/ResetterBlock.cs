using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class ResetterBlock : BasicBlock
{
    public bool Status { get; }
    
    public ResetterBlock(int x, int y, int layer, int blockId, bool activated) : base(x, y, layer, blockId)
    {
        Status = activated;
    }

    public override IPixelGamePacketOut AsPacketOut()
    {
        return new WorldBlockPlacedOutPacket(X, Y, Layer, BlockId, null, null, null, Convert.ToByte(Status));
    }
}