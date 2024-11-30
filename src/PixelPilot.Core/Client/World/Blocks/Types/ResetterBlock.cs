using System.Drawing;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Send;
using PixelPilot.Client.World.Constants;

namespace PixelPilot.Client.World.Blocks.Types;

public class ResetterBlock : BasicBlock
{
    public bool Status { get; }
    
    public ResetterBlock(int blockId, bool activated) : base(blockId)
    {
        Status = activated;
    }
    
    public ResetterBlock(PixelBlock block, bool activated) : this((int) block, activated)
    {
        
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Convert.ToByte(Status)]);
    }

    public override IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer)
    {
        return new WorldBlockPlacedOutPacket(positions, layer, BlockId, [Convert.ToByte(Status)]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Convert.ToByte(Status));

        return memoryStream.ToArray();
    }

    protected bool Equals(ResetterBlock other)
    {
        return base.Equals(other) && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ResetterBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Status);
    }
}