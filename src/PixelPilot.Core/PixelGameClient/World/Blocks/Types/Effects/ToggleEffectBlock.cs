using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks.Effects;

public class ToggleEffectBlock : BasicBlock
{
    public bool Enabled { get; set; }
    
    public ToggleEffectBlock(int blockId, bool enabled) : base(blockId)
    {
        Enabled = enabled;
    }

    public ToggleEffectBlock(PixelBlock block, bool enabled) : base(block)
    {
        Enabled = enabled;
    }
    
    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Enabled]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Enabled);

        return memoryStream.ToArray();
    }

    protected bool Equals(ToggleEffectBlock other)
    {
        return base.Equals(other) && Enabled == other.Enabled;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals( (ToggleEffectBlock) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Enabled);
    }
}