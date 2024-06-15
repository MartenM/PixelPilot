using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerLocalSwitchChangedOutPacket : ReflectivePixelOutPacket
{
    public int SwitchId { get; }
    public byte Enabled { get; }
    
    public PlayerLocalSwitchChangedOutPacket(int switchId, bool enabled) : base(WorldMessageType.PlayerLocalSwitchChanged)
    {
        SwitchId = switchId;
        Enabled = enabled ? (byte) 1 : (byte) 0;
    }
}