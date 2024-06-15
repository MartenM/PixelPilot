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
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Direction, PortalId, TargetId]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Direction);
        writer.Write(PortalId);
        writer.Write(TargetId);

        return memoryStream.ToArray();
    }

    protected bool Equals(PortalBlock other)
    {
        return base.Equals(other) && PortalId == other.PortalId && TargetId == other.TargetId && Direction == other.Direction;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PortalBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), PortalId, TargetId, Direction);
    }
}