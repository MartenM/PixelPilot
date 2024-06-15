using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class GlobalSwitchResetOutPacket : ReflectivePixelOutPacket
{
    public byte Enabled { get; }
    
    public GlobalSwitchResetOutPacket(bool enabled) : base(WorldMessageType.GlobalSwitchReset)
    {
        Enabled = Convert.ToByte(enabled);
    }
}