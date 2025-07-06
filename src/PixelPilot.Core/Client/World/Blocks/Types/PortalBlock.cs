using System.Drawing;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types;

public class PortalBlock : BasicBlock
{
    public string PortalId { get; set; }
    public string TargetId { get; set; }
    
    public int Direction { get; set; }
    
    public PortalBlock(int blockId, string portalId, string targetId, int direction) : base(blockId)
    {
        PortalId = portalId;
        TargetId = targetId;
        Direction = direction;
    }

    public PortalBlock(PixelBlock block, string portalId, string targetId, int direction) : this((int)block, portalId, targetId, direction)
    {
        
    }

    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [Direction, PortalId, TargetId]);
    }

    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [Direction, PortalId, TargetId]);
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