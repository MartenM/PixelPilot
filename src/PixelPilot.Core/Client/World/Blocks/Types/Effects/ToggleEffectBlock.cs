using System.Drawing;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types.Effects;

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
    
    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [Enabled]);
    }

    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [Enabled]);
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