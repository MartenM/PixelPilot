using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

public class PlayerEffectRemovedOutPacket : ReflectivePixelOutPacket
{
    public int EffectId { get; set; }
    
    public PlayerEffectRemovedOutPacket(int effectId) : base(WorldMessageType.PlayerRemoveEffect)
    {
        EffectId = effectId;
    }
}