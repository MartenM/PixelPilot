namespace PixelPilot.PixelGameClient.Messages.Received;

/// <summary>
/// Received when a player actives a block. This can be done manually or when a player touched the block.
/// </summary>
public class PlayerTouchBlockPacket : IPixelGamePlayerPacket
{
    public int PlayerId { get; }
    public int X { get; }
    public int Y { get; }
    public int BlockId { get; }
    
    public PlayerTouchBlockPacket(int id, int x, int y, int blockId)
    {
        PlayerId = id;
        X = x;
        Y = y;
        BlockId = blockId;
    }
}