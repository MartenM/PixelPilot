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
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, Morph);
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
}