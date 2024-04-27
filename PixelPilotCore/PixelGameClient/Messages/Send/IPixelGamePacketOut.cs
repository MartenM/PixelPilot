namespace PixelPilot.PixelGameClient.Messages.Send;

public interface IPixelGamePacketOut
{
    public byte[] ToBinaryPacket();
}