namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerModMode : IPixelGamePacket
{
    public PlayerModMode(int id, bool enabled)
    {
        Id = id;
        Enabled = enabled;
    }

    public int Id { get; }
    public bool Enabled { get; }
}