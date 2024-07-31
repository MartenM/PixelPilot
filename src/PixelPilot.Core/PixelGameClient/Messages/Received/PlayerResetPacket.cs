using System.Drawing;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerResetPacket : IPixelGamePlayerPacket
{
    public PlayerResetPacket(int id, int x, int y)
    {
        PlayerId = id;
        Position = new Point(x, y);
    }
    
    public PlayerResetPacket(int id)
    {
        PlayerId = id;
        Position = null;
    }

    public int PlayerId { get; }
    public Point? Position { get; set; }
}