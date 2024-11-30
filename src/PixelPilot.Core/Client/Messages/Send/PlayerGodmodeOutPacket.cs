using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerGodmodeOutPacket : ReflectivePixelOutPacket
{
    public bool Enabled { get; set; }
    
    public PlayerGodmodeOutPacket(bool enabled) : base(WorldMessageType.PlayerGodMode)
    {
        Enabled = enabled;
    }
}