using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerTouchBlockOutPacket : ReflectivePixelOutPacket
{
    public int X { get; set; }
    public int Y { get; set; }
    public int BlockId { get; set; }
    
    public PlayerTouchBlockOutPacket(int x, int y, int blockId) : base(WorldMessageType.PlayerTouchBlock)
    {
        X = x;
        Y = y;
        BlockId = blockId;
    }
}