using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerEffectRemovedOutPacket : ReflectivePixelOutPacket
{
    public int EffectId { get; set; }
    
    public PlayerEffectRemovedOutPacket(int effectId) : base(WorldMessageType.PlayerRemoveEffect)
    {
        EffectId = effectId;
    }
}