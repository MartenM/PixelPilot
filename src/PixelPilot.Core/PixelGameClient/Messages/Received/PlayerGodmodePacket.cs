using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerGodmodePacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public PlayerGodmodePacket(int id, bool isEnabled)
    {
        PlayerId = id;
        IsEnabled = isEnabled;
    }

    public int PlayerId { get; }
    public bool IsEnabled { get; }
    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerGodmodeOutPacket(IsEnabled);
    }
}