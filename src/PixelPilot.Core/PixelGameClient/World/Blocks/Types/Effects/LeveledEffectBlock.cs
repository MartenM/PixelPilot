using System.Drawing;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks.Effects;

public class LeveledEffectBlock : BasicBlock
{
    public int Level { get; set; }
    
    public LeveledEffectBlock(int blockId, int level) : base(blockId)
    {
        Level = level;
    }

    public LeveledEffectBlock(PixelBlock block, int level) : base(block)
    {
        Level = level;
    }
    
    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Level]);
    }

    public override IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer)
    {
        return new WorldBlockPlacedOutPacket(positions, layer, BlockId, [Level]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Level);

        return memoryStream.ToArray();
    }

    protected bool Equals(LeveledEffectBlock other)
    {
        return base.Equals(other) && Level == other.Level;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LeveledEffectBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Level);
    }
}