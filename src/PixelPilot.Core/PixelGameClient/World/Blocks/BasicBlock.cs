using System.Drawing;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Basic blocks only have a block ID.
/// No special data can be assigned to them.
/// </summary>
public class BasicBlock : IPixelBlock
{
    public BasicBlock(int blockId)
    {
        BlockId = blockId;
    }
    
    public BasicBlock(PixelBlock block)
    {
        BlockId = (int) block;
    }
    
    public int BlockId { get; set; }
    
    public virtual IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x,y, layer, BlockId);
    }

    public virtual IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer)
    {
        return new WorldBlockPlacedOutPacket(positions, layer, BlockId);
    }

    public IPlacedBlock AsPlacedBlock(int x, int y, int layer)
    {
        return new PlacedBlock(x, y, layer, this);
    }
    
    public byte[] AsWorldBuffer(int x, int y, int layer)
    {
        return AsWorldBuffer(x, y, layer, BlockId);
    }

    public virtual byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);

        return memoryStream.ToArray();
    }

    protected bool Equals(BasicBlock other)
    {
        return BlockId == other.BlockId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BasicBlock)obj);
    }

    public override int GetHashCode()
    {
        return BlockId;
    }
}