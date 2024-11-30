using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerMoveOutPacket : ReflectivePixelOutPacket
{
    public PlayerMoveOutPacket(double x, double y, double velocityX, double velocityY, double modX, double modY, int horizontal, int vertical, bool spacedown, bool spaceJustDown, bool justTeleported, int tick) : base(WorldMessageType.PlayerMoved)
    {
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
        justTeleported = justTeleported;
        Tick = tick;
    }
    public double X { get; set; }
    public double Y { get; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
    public double ModX { get; set; }
    public double ModY { get; set; }
    public int Horizontal { get; set; }
    public int Vertical { get; set; }
    public bool Spacedown { get; set; }
    public bool SpaceJustDown { get; set; }
    public bool JustTeleported { get; set; }
    public int Tick { get; set; }
}