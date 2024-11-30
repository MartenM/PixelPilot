using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class GlobalSwitchResetOutPacket : ReflectivePixelOutPacket
{
    public byte Enabled { get; set; }
    
    public GlobalSwitchResetOutPacket(bool enabled) : base(WorldMessageType.GlobalSwitchReset)
    {
        Enabled = Convert.ToByte(enabled);
    }
    
    public GlobalSwitchResetOutPacket() : base(WorldMessageType.GlobalSwitchReset)
    {
        // Utility constructor
    }
}