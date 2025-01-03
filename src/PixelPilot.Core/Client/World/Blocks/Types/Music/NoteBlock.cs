using System.Drawing;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Types.Music;

public class NoteBlock : BasicBlock
{
    protected byte[] Notes;

    public NoteBlock(int blockId, byte[] notes) : base(blockId)
    {
        Notes = notes;
    }

    public NoteBlock(PixelBlock block, byte[] notes) : base(block)
    {
        Notes = notes;
    }
    
    public override WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(x, y, layer, BlockId, [Notes]);
    }

    public override WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        return WorldBlockPacketBuilder.CreatePacket(positions, layer, BlockId, [Notes]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        
        writer.Write7BitEncodedInt(Notes.Length);
        writer.Write(Notes);

        return memoryStream.ToArray();
    }

    protected bool Equals(NoteBlock other)
    {
        return base.Equals(other) && Notes.SequenceEqual(other.Notes);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((NoteBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Notes);
    }
}