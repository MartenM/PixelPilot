using PixelPilot.Client.Messages.Send;

namespace PixelPilot.Client.Messages.Received;

public class GlobalSwitchResetPacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public GlobalSwitchResetPacket(int playerId, byte enabled)
    {
        PlayerId = playerId;
        Enabled = enabled == 1;
    }

    public int PlayerId { get; }
    public bool Enabled { get; }
    public IPixelGamePacketOut AsPacketOut()
    {
        return new GlobalSwitchResetOutPacket(Enabled);
    }
}