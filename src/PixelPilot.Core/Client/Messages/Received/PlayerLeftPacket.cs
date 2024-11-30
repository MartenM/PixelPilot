namespace PixelPilot.Client.Messages.Received;

public class PlayerLeftPacket : IPixelGamePlayerPacket
{
    public int PlayerId { get; }
    
    public PlayerLeftPacket(int id)
    {
        PlayerId = id;
    }
}