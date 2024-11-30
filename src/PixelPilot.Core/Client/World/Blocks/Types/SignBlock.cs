﻿using System.Drawing;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Send;
using PixelPilot.Client.World.Constants;

namespace PixelPilot.Client.World.Blocks.Types;

public class SignBlock : BasicBlock
{
    public string Text { get; set; }

    public SignBlock(int blockId, string text) : base(blockId)
    {
        Text = text;
    }

    public SignBlock(PixelBlock block, string text) : base(block)
    {
        Text = text;
    }

    public override IPixelGamePacketOut AsPacketOut(int x, int y, int layer)
    {
        return new WorldBlockPlacedOutPacket(x, y, layer, BlockId, [Text]);
    }

    public override IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer)
    {
        return new WorldBlockPlacedOutPacket(positions, layer, BlockId, [Text]);
    }

    public override byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(Text);

        return memoryStream.ToArray();
    }

    protected bool Equals(SignBlock other)
    {
        return base.Equals(other) && Text == other.Text;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SignBlock)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Text);
    }
}