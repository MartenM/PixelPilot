using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerResetEffectsPacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public PlayerResetEffectsPacket(int id, bool magic)
    {
        PlayerId = id;
        Magic = magic;
    }

    public int PlayerId { get; }
    
    public bool Magic { get; }
    
    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerResetEffects();
    }
}