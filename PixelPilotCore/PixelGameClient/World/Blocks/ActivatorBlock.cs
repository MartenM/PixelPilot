using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

public class ActivatorBlock : BasicBlock
{
    public int SwitchId { get; set; }
    public bool Status { get; set; }
    public ActivatorBlock(int blockId, int switchId, bool status) : base(blockId)
    {
        SwitchId = switchId;
        Status = status;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, SwitchId, null, null, Convert.ToByte(Status));
    }
}