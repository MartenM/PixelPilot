namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerResetEffectsPacket : IPixelGamePlayerPacket
{
    public PlayerResetEffectsPacket(int id)
    {
        PlayerId = id;
    }

    public int PlayerId { get; }
}