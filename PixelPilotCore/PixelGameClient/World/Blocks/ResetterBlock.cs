using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class ResetterBlock : BasicBlock
{
    public bool Status { get; }
    
    public ResetterBlock(int blockId, bool activated) : base(blockId)
    {
        Status = activated;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, null, null, null, Convert.ToByte(Status));
    }
}