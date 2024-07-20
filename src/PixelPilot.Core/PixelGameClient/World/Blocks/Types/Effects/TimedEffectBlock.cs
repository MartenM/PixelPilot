using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks.Effects;

public class TimedEffectBlock : BasicBlock
{
    public int Duration { get; set; }
    
    public TimedEffectBlock(int blockId, int duration) : base(blockId)
    {
        Duration = duration;
    }

    public TimedEffectBlock(PixelBlock block, int duration) : base(block)
    {
        Duration = duration;
    }
    
    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Duration]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Duration);

        return memoryStream.ToArray();
    }

    protected bool Equals(TimedEffectBlock other)
    {
        return base.Equals(other) && Duration == other.Duration;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TimedEffectBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Duration);
    }
}