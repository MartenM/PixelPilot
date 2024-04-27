namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerLeavePacket : IPixelGamePacket
{
    private int Id { get; }
    
    public PlayerLeavePacket(int id)
    {
        Id = id;
    }
}