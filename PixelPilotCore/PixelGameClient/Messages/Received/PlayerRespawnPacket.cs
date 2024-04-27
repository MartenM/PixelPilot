namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerRespawnPacket : IPixelGamePacket
{
    public PlayerRespawnPacket(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public int Id { get; }
    public int X { get; }
    public int Y { get; }
}