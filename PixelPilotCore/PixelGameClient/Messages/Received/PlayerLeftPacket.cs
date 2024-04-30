namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerLeftPacket : IPixelGamePacket
{
    public int PlayerId { get; }
    
    public PlayerLeftPacket(int id)
    {
        PlayerId = id;
    }
}