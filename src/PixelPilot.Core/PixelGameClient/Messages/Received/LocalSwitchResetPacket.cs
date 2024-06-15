using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class LocalSwitchResetPacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public LocalSwitchResetPacket(int playerId, byte enabled)
    {
        PlayerId = playerId;
        Enabled = enabled == 1;
    }

    public int PlayerId { get; }
    public bool Enabled { get; }
    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerLocalSwitchResetOutPacket(Enabled);
    }
}