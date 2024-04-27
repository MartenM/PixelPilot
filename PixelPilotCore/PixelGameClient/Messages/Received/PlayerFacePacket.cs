namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerFacePacket : IPixelGamePacket
{
    public PlayerFacePacket(int id, int face)
    {
        Id = id;
        Face = face;
    }

    public int Id { get; }
    public int Face { get; }
}