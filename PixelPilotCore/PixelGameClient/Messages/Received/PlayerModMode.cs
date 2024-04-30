namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerModMode : IPixelGamePacket
{
    public PlayerModMode(int id, bool enabled)
    {
        PlayerId = id;
        Enabled = enabled;
    }

    public int PlayerId { get; }
    public bool Enabled { get; }
}