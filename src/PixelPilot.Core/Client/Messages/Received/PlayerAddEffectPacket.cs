using PixelPilot.Client.Messages.Constants;
using PixelPilot.Client.Messages.Send;

namespace PixelPilot.Client.Messages.Received;

public class PlayerAddEffectPacket : IPixelGamePlayerPacket, IDynamicConstructedPacket, IPacketOutConvertible
{
    public int PlayerId { get; }
    
    public int EffectId { get; }
    public EffectType EffectType => (EffectType) EffectId;
    
    public bool ActivatedByPlayer { get; }
    
    public dynamic[] ExtraFields { get; }
    
    public PlayerAddEffectPacket(List<dynamic> fields)
    {
        PlayerId = fields[0];
        ActivatedByPlayer = fields[1];
        EffectId = fields[2];

        ExtraFields = fields.Skip(3).ToArray();
    }

    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerAddEffectOutPacket(EffectId, ExtraFields);
    }
}