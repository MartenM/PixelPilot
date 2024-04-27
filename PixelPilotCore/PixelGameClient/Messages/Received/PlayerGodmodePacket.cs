namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerGodmodePacket : IPixelGamePacket
{
    public PlayerGodmodePacket(int id, bool isEnabled)
    {
        Id = id;
        IsEnabled = isEnabled;
    }

    public int Id { get; }
    public bool IsEnabled { get; }
}