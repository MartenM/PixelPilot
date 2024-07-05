using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class GlobalSwitchChangeOutPacket : ReflectivePixelOutPacket
{
    public int SwitchId { get; set; }
    public byte Enabled { get; set; }
    
    public GlobalSwitchChangeOutPacket(int switchId, bool enabled) : base(WorldMessageType.GlobalSwitchChanged)
    {
        SwitchId = switchId;
        Enabled = Convert.ToByte(enabled);
    }
    
    public GlobalSwitchChangeOutPacket() : base(WorldMessageType.GlobalSwitchChanged)
    {
        // Utility constructor
    }
}