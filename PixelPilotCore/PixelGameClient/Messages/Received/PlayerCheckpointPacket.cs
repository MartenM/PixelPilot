namespace PixelPilot.PixelGameClient.Messages.Received;

/// <summary>
/// Send by the server to force change a checkpoint position.
/// When -1,-1 no checkpoint is set.
/// </summary>
public class CheckpointPacket : IPixelGamePacket
{
    public CheckpointPacket(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }
}