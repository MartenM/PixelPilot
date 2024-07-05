using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerLocalSwitchChangedOutPacket : ReflectivePixelOutPacket
{
    public int SwitchId { get; set; }
    public byte Enabled { get; set; }
    
    public PlayerLocalSwitchChangedOutPacket(int switchId, bool enabled) : base(WorldMessageType.PlayerLocalSwitchChanged)
    {
        SwitchId = switchId;
        Enabled = enabled ? (byte) 1 : (byte) 0;
    }
    
    public PlayerLocalSwitchChangedOutPacket() : base(WorldMessageType.PlayerLocalSwitchChanged)
    {
       // Utility constructor
    }
}