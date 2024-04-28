using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Morphable blocks include blocks with multiple states.
/// This includes decorations but also coin doors, gates, etc.
/// </summary>
public class MorphableBlock : BasicBlock
{
    public int Morph { get; set; }
    
    public MorphableBlock(int x, int y, int layer, int blockId, int morph) : base(x, y, layer, blockId)
    {
        Morph = morph;
    }

    public override IPixelGamePacketOut AsPacketOut()
    {
        return new WorldBlockPlacedOutPacket(X, Y, Layer, BlockId, Morph);
    }
}