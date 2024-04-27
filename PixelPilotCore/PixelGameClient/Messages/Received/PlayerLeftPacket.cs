namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerLeftPacket : IPixelGamePacket
{
    public int Id { get; }
    
    public PlayerLeftPacket(int id)
    {
        Id = id;
    }
}