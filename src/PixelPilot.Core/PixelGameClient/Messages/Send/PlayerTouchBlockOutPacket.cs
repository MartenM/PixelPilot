using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerTouchBlockOutPacket : ReflectivePixelOutPacket
{
    public int X { get; set; }
    public int Y { get; set; }
    public int BlockId { get; set; }
    
    public PlayerTouchBlockOutPacket() : base(WorldMessageType.PlayerTouchBlock) {}
}