using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Blocks;

/// <summary>
/// Generic interface for blocks. Every block has atleast these properties.
/// </summary>
public interface IPixelBlock
{
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    public int BlockId { get; }
    public PixelBlock Block => (PixelBlock) BlockId;

    public IPixelGamePacketOut AsPacketOut();
}