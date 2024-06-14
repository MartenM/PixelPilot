using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerGodmodeOutPacket : ReflectivePixelOutPacket
{
    public bool Enabled { get; set; }
    
    public PlayerGodmodeOutPacket(bool enabled) : base(WorldMessageType.PlayerGodMode)
    {
        Enabled = enabled;
    }
}