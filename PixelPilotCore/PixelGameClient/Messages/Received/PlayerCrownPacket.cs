namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerCrownPacket : IPixelGamePacket
{
    public PlayerCrownPacket(int id)
    {
        PlayerId = id;
    }

    public int PlayerId { get; }
}