namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerTeleportedPacket : IPixelGamePlayerPacket
{
    public PlayerTeleportedPacket(int id, double x, double y)
    {
        PlayerId = id;
        X = x;
        Y = y;
    }

    public int PlayerId { get; }
    public double X { get; }
    public double Y { get; }
    
    
}