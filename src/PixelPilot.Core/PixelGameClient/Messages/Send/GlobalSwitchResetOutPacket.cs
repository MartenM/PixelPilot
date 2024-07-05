using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

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