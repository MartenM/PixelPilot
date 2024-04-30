namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerResetPacket : IPixelGamePacket
{
    public PlayerResetPacket(int id, int x, int y)
    {
        PlayerId = id;
        X = x;
        Y = y;
    }

    public int PlayerId { get; }
    public int X { get; }
    public int Y { get; }
}