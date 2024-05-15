namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerModMode : IPixelGamePlayerPacket
{
    public PlayerModMode(int id, bool isEnabled)
    {
        PlayerId = id;
        IsEnabled = isEnabled;
    }

    public int PlayerId { get; }
    public bool IsEnabled { get; }
}