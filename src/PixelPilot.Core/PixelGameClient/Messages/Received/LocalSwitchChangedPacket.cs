using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Received;

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