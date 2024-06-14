namespace PixelPilot.PixelGameClient.Messages;

public interface IPacketOutConvertible
{
    public IPixelGamePacketOut AsPacketOut();
}