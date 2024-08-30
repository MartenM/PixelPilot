using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerAddEffectOutPacket : ReflectivePixelOutPacket
{
    public int EffectId { get; set; }
    public dynamic[]? ExtraFields { get; } = null;
    
    public PlayerAddEffectOutPacket(int effectId, dynamic[]? extraFields) : base(WorldMessageType.PlayerAddEffect)
    {
        EffectId = effectId;
        ExtraFields = extraFields;
    }
    
    protected override List<dynamic> GetFields()
    {
        return GetFields(true);
    }
}