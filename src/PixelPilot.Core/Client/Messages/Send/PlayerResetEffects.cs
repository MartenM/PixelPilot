using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerResetEffects : ReflectivePixelOutPacket
{
    public PlayerResetEffects() : base(WorldMessageType.PlayerResetEffects)
    {
        
    }
}