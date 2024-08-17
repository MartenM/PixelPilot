namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerResetEffectsPacket : IPixelGamePlayerPacket
{
    public PlayerResetEffectsPacket(int id, bool magic)
    {
        PlayerId = id;
        Magic = magic;
    }

    public int PlayerId { get; }
    
    public bool Magic { get; }
}