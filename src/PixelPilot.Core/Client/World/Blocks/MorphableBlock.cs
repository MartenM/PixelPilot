using System.Drawing;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks;

/// <summary>
/// Morphable blocks include blocks with multiple states.
/// This includes decorations but also coin doors, gates, etc.
/// </summary>
[Obsolete("Use FlexBlock instead")]
public class MorphableBlock : BasicBlock
{
    public int Morph { get; set; }
    
    public MorphableBlock(int blockId, int morph) : base(blockId)
    {
        Morph = morph;
    }
    
    public MorphableBlock(PixelBlock block, int morph) : base(block)
    {
        Morph = morph;
    }

    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [Morph]);
    }

    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [Morph]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Morph);

        return memoryStream.ToArray();
    }

    protected bool Equals(MorphableBlock other)
    {
        return base.Equals(other) && Morph == other.Morph;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MorphableBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Morph);
    }
}