namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerFacePacket : IPixelGamePacket
{
    public PlayerFacePacket(int id, int face)
    {
        PlayerId = id;
        Face = face;
    }

    public int PlayerId { get; }
    public int Face { get; }
}