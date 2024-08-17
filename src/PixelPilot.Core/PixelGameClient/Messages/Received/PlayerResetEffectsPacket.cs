namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerResetEffectsPacket : IPixelGamePlayerPacket
{
    public PlayerResetEffectsPacket(int id, bool byServer)
    {
        PlayerId = id;
        ByServer = byServer;
    }

    public int PlayerId { get; }
    
    public bool ByServer { get; }
}