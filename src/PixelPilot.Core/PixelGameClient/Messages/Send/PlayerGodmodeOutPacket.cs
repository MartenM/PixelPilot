using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerGodmodeOutPacket : ReflectivePixelOutPacket
{
    private bool Enabled { get; set; }
    
    public PlayerGodmodeOutPacket(bool enabled) : base(WorldMessageType.PlayerGodMode)
    {
        Enabled = enabled;
    }
}