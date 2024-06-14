using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class GlobalSwitchChangeOutPacket : ReflectivePixelOutPacket
{
    public int SwitchId { get; }
    public byte Enabled { get; }
    
    public GlobalSwitchChangeOutPacket(int switchId, bool enabled) : base(WorldMessageType.GlobalSwitchChanged)
    {
        SwitchId = switchId;
        Enabled = Convert.ToByte(enabled);
    }
}