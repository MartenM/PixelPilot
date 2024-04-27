namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerMovePacket : IPixelGamePacket
{
    public PlayerMovePacket(int id, double x, double y, double velocityX, double velocityY, double modX, double modY, int horizontal, int vertical, bool spacedown, bool spaceJustDown, int tickId)
    {
        Id = id;
        X = x;
        Y = y;
        VelocityX = velocityX;
        VelocityY = velocityY;
        ModX = modX;
        ModY = modY;
        Horizontal = horizontal;
        Vertical = vertical;
        Spacedown = spacedown;
        SpaceJustDown = spaceJustDown;
        TickId = tickId;
    }

    public int Id { get; }
    public double X { get; }
    public double Y { get; }
    public double VelocityX { get; }
    public double VelocityY { get; }
    public double ModX { get; }
    public double ModY { get; }
    public int Horizontal { get; }
    public int Vertical { get; }
    public bool Spacedown { get; }
    public bool SpaceJustDown { get; }
    public int TickId { get; }
}