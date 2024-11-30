using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerLocalSwitchResetOutPacket: ReflectivePixelOutPacket
{
    public byte Enabled { get; set; }
    
    public PlayerLocalSwitchResetOutPacket(bool enabled) : base(WorldMessageType.PlayerLocalSwitchReset)
    {
        Enabled = Convert.ToByte(enabled);
    }
    
    public PlayerLocalSwitchResetOutPacket() : base(WorldMessageType.PlayerLocalSwitchReset)
    {
        // Utility constructor
    }
}