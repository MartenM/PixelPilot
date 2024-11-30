using PixelPilot.Client.Messages.Send;

namespace PixelPilot.Client.Messages.Received;

public class LocalSwitchChangedPacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public LocalSwitchChangedPacket(int playerId, int switchId, byte enabled)
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
        return new PlayerLocalSwitchChangedOutPacket(SwitchId, Enabled);
    }
}