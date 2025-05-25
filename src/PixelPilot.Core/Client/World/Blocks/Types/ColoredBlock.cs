using System.Drawing;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types;

public class ColoredBlock : BasicBlock
{
    public Color PrimaryColor { get; set; }
    
    public ColoredBlock(PixelBlock block, Color color) : base(block)
    {
        PrimaryColor = color;
    }
    
    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        uint rawColor = 0;
        rawColor |= ((uint) PrimaryColor.A << 24);
        rawColor |= ((uint) PrimaryColor.R << 16);
        rawColor |= ((uint) PrimaryColor.G << 8);
        rawColor |= ((uint) PrimaryColor.B);
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [rawColor]);
    }
    
    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        uint rawColor = 0;
        rawColor |= ((uint) PrimaryColor.A << 24);
        rawColor |= ((uint) PrimaryColor.R << 16);
        rawColor |= ((uint) PrimaryColor.G << 8);
        rawColor |= ((uint) PrimaryColor.B);
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [rawColor]);
    }
    
    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        
        uint rawColour  =
            ((uint) PrimaryColor.A << 24) |
            ((uint) PrimaryColor.R << 16) |
            ((uint) PrimaryColor.G << 8) |
            ((uint) PrimaryColor.B);
        
        writer.Write(rawColour);

        return memoryStream.ToArray();
    }

    protected bool Equals(ColoredBlock other)
    {
        return base.Equals(other) && PrimaryColor.Equals(other.PrimaryColor);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ColoredBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), PrimaryColor);
    }
}