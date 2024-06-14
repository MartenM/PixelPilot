using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerLocalSwitchResetOutPacket: ReflectivePixelOutPacket
{
    public byte Enabled { get; }
    
    public PlayerLocalSwitchResetOutPacket(bool enabled) : base(WorldMessageType.PlayerLocalSwitchReset)
    {
        Enabled = Convert.ToByte(enabled);
    }
}