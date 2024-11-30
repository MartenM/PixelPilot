using PixelPilot.Client.Messages;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.Placed;

/// <summary>
/// A placed block has a location assigned to it.
/// This wrapper is used by API methods where the block in question is placed in some kind of way.
/// </summary>
public interface IPlacedBlock : ICloneable
{
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    public IPixelBlock Block { get; }

    public WorldBlockPlacedPacket AsPacketOut();

    public byte[] AsWorldBuffer();
    public byte[] AsWorldBuffer(int customId);
}