using System.Drawing;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Generic interface for blocks. Every block has atleast these properties.
/// </summary>
public interface IPixelBlock : ICloneable
{
    public int BlockId { get; }
    public PixelBlock Block => (PixelBlock) BlockId;

    public IPixelGamePacketOut AsPacketOut(int x, int y, int layer);
    public IPixelGamePacketOut AsPacketOut(List<Point> positions, int layer);
    public IPlacedBlock AsPlacedBlock(int x, int y, int layer);

    public byte[] AsWorldBuffer(int x, int y, int layer);
    public byte[] AsWorldBuffer(int x, int y, int layer, int customId);
}