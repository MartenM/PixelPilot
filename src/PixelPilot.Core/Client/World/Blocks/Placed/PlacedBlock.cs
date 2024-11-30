using PixelPilot.Client.Messages;
using PixelPilot.Client.World.Constants;

namespace PixelPilot.Client.World.Blocks.Placed;

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
    
    public PlacedBlock(int x, int y, WorldLayer layer, IPixelBlock block)
    {
        X = x;
        Y = y;
        Layer = (int) layer;
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

    protected bool Equals(PlacedBlock other)
    {
        return X == other.X && Y == other.Y && Layer == other.Layer && Block.Equals(other.Block);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PlacedBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Layer, Block);
    }

    public object Clone()
    {
        return new PlacedBlock(X, Y, Layer, (IPixelBlock) Block.Clone());
    }
}