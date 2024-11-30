using PixelPilot.Client.Messages.Send;

namespace PixelPilot.Client.Messages.Received;

public class GlobalSwitchChangedPacket : IPixelGamePlayerPacket, IPacketOutConvertible
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
    public IPixelGamePacketOut AsPacketOut()
    {
        return new GlobalSwitchChangeOutPacket(SwitchId, Enabled);
    }
}