using PixelPilot.Client.Messages.Send;

namespace PixelPilot.Client.Messages.Received;

public class PlayerFacePacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public PlayerFacePacket(int id, int face)
    {
        PlayerId = id;
        Face = face;
    }

    public int PlayerId { get; }
    public int Face { get; }
    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerFaceOutPacket(Face);
    }
}