using PixelPilot.PixelGameClient.Messages;

namespace PixelPilot.PixelGameClient.World.Blocks.Placed;

/// <summary>
/// A placed block has a location assigned to it.
/// This wrapper is used by API methods where the block in question is placed in some kind of way.
/// </summary>
public interface IPlacedBlock
{
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    public IPixelBlock Block { get; }

    public IPixelGamePacketOut AsPacketOut();

    public byte[] AsWorldBuffer();
    public byte[] AsWorldBuffer(int customId);
}