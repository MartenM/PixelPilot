using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerChatOutPacket : ReflectivePixelOutPacket
{
    public string Message { get; set; }
    
    public PlayerChatOutPacket(string message) : base(WorldMessageType.PlayerChatMessage)
    {
        Message = message;
    }
}