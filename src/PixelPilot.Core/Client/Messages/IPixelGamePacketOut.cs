namespace PixelPilot.Client.Messages;

/// <summary>
/// Packets that can be sent to the PixelWalker game server.
/// </summary>
public interface IPixelGamePacketOut
{
    public byte[] ToBinaryPacket();
}