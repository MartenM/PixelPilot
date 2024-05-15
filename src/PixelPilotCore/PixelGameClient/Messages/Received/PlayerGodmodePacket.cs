namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerGodmodePacket : IPixelGamePlayerPacket
{
    public PlayerGodmodePacket(int id, bool isEnabled)
    {
        PlayerId = id;
        IsEnabled = isEnabled;
    }

    public int PlayerId { get; }
    public bool IsEnabled { get; }
}