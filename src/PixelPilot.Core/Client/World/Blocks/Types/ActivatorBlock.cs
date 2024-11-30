using System.Drawing;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types;

public class ActivatorBlock : BasicBlock
{
    public int SwitchId { get; set; }
    public bool Status { get; set; }
    public ActivatorBlock(int blockId, int switchId, bool status) : base(blockId)
    {
        SwitchId = switchId;
        Status = status;
    }
    
    public ActivatorBlock(PixelBlock block, int switchId, bool status) : this((int) block, switchId, status)
    {

    }

    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [SwitchId, Convert.ToByte(Status)]);
    }

    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [SwitchId, Convert.ToByte(Status)]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(SwitchId);
        writer.Write(Convert.ToByte(Status));

        return memoryStream.ToArray();
    }

    protected bool Equals(ActivatorBlock other)
    {
        return base.Equals(other) && SwitchId == other.SwitchId && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ActivatorBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), SwitchId, Status);
    }
}