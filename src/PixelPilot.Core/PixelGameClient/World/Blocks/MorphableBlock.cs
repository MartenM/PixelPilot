using System.Drawing;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Morphable blocks include blocks with multiple states.
/// This includes decorations but also coin doors, gates, etc.
/// </summary>
public class MorphableBlock : BasicBlock
{
    public int Morph { get; set; }
    
    public MorphableBlock(int blockId, int morph) : base(blockId)
    {
        Morph = morph;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Morph]);
    }

    public override IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer)
    {
        return new WorldBlockPlacedOutPacket(positions, layer, BlockId, [Morph]);
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