namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerCrownPacket : IPixelGamePacket
{
    public PlayerCrownPacket(int id)
    {
        Id = id;
    }

    public int Id { get; }
}