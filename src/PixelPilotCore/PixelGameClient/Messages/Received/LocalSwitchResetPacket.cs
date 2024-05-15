namespace PixelPilot.PixelGameClient.Messages.Received;

public class LocalSwitchResetPacket : IPixelGamePacket
{
    public LocalSwitchResetPacket(int switchId, byte enabled)
    {
        SwitchId = switchId;
        Enabled = enabled == 1;
    }

    public int SwitchId { get; }
    public bool Enabled { get; }
}