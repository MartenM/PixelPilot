namespace PixelPilot.Client.Messages;

public interface IPacketOutConvertible
{
    public IPixelGamePacketOut AsPacketOut();
}