namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerCrownPacket : IPixelGamePlayerPacket
{
    public PlayerCrownPacket(int id)
    {
        PlayerId = id;
    }

    public int PlayerId { get; }
}