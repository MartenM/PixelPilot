using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerChatOutPacket : ReflectivePixelOutPacket
{
    public string Message { get; set; }
    
    public PlayerChatOutPacket(string message) : base(WorldMessageType.PlayerChatMessage)
    {
        Message = message;
    }
}