using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Received;

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