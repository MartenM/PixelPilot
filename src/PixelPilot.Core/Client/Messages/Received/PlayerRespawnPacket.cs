namespace PixelPilot.Client.Messages.Received;

public class PlayerRespawnPacket : IPixelGamePlayerPacket
{
    public PlayerRespawnPacket(int id, int x, int y)
    {
        PlayerId = id;
        X = x;
        Y = y;
    }

    public int PlayerId { get; }
    public int X { get; }
    public int Y { get; }
}