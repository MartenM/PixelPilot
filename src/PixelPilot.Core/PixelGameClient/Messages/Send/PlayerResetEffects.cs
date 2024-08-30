using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerResetEffects : ReflectivePixelOutPacket
{
    public PlayerResetEffects() : base(WorldMessageType.PlayerResetEffects)
    {
        
    }
}