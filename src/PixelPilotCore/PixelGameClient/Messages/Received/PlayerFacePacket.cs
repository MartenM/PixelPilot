namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerFacePacket : IPixelGamePlayerPacket
{
    public PlayerFacePacket(int id, int face)
    {
        PlayerId = id;
        Face = face;
    }

    public int PlayerId { get; }
    public int Face { get; }
}