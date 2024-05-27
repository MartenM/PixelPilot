using PixelPilot.PixelGameClient.Messages;

namespace PixelPilot.PixelGameClient.World.Blocks.Placed;

/// <summary>
/// An immutable instance of a <see cref="IPlacedBlock"/>
/// </summary>
public class PlacedBlock : IPlacedBlock
{
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    
    public int? PlacedUserId { get; }

    public IPixelBlock Block { get; }

    public PlacedBlock(int x, int y, int layer, IPixelBlock block)
    {
        X = x;
        Y = y;
        Layer = layer;
        Block = block;
    }
    
    public IPixelGamePacketOut AsPacketOut()
    {
        return Block.AsPacketOut(X, Y, Layer);
    }

    public byte[] AsWorldBuffer()
    {
        return Block.AsWorldBuffer(X, Y, Layer);
    }

    public byte[] AsWorldBuffer(int customId)
    {
        return Block.AsWorldBuffer(X, Y, Layer, customId);
    }
}