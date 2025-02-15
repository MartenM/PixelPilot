using System.Drawing;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types;

public class ColoredBlock : BasicBlock
{
    public uint RawBlockColor { get; }
    
    public Color BlockColor => Color.FromArgb(
        (int)((RawBlockColor ) & 0xFF), // Alpha
        (int)((RawBlockColor >> 8 ) & 0xFF), // Red
        (int)((RawBlockColor >> 16 ) & 0xFF),  // Green
        (int)(RawBlockColor >> 24 & 0xFF)          // Blue
    );
    
    public ColoredBlock(int blockId, uint color) : base(blockId)
    {
        RawBlockColor = color;
    }
    
    public ColoredBlock(PixelBlock block, uint color) : base(block)
    {
        RawBlockColor = color;
    }
    
    public ColoredBlock(PixelBlock block, Color color) : base(block)
    {
        RawBlockColor = ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | color.B;
    }
    
    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [RawBlockColor]);
    }
    
    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [RawBlockColor]);
    }
    
    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(RawBlockColor);

        return memoryStream.ToArray();
    }
}