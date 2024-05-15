namespace PixelPilot.PixelGameClient.Messages.Received;

public class GlobalSwitchChangedPacket : IPixelGamePacket
{
    public GlobalSwitchChangedPacket(int playerId, int switchId, byte enabled)
    {
        PlayerId = playerId;
        SwitchId = switchId;
        Enabled = enabled == 1;
    }

    public int PlayerId { get; }
    public int SwitchId { get; }
    public bool Enabled { get; }
}