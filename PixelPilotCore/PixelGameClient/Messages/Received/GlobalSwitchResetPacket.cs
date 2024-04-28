namespace PixelPilot.PixelGameClient.Messages.Received;

public class GlobalSwitchResetPacket : IPixelGamePacket
{
    public GlobalSwitchResetPacket(int switchId, byte enabled)
    {
        SwitchId = switchId;
        Enabled = enabled == 1;
    }

    public int SwitchId { get; }
    public bool Enabled { get; }
}