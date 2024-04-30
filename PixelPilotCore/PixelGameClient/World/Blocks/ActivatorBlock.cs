using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class ActivatorBlock : BasicBlock
{
    public int SwitchId { get; set; }
    public bool Status { get; set; }
    public ActivatorBlock(int x, int y, int layer, int blockId, int switchId, bool status) : base(x, y, layer, blockId)
    {
        SwitchId = switchId;
        Status = status;
    }

    public override IPixelGamePacketOut AsPacketOut()
    {
        return new WorldBlockPlacedOutPacket(X, Y, Layer, BlockId, SwitchId, null, null, Convert.ToByte(Status));
    }
}