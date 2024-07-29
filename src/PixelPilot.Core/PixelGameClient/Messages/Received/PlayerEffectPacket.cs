using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerEffectPacket : IPixelGamePlayerPacket, IDynamicConstructedPacket
{
    public int PlayerId { get; }
    
    public int EffectId { get; }
    public EffectType EffectType => (EffectType) EffectId;
    
    public bool Magic { get; }
    
    public dynamic[] ExtraFields { get; }
    
    public PlayerEffectPacket(List<dynamic> fields)
    {
        PlayerId = fields[0];
        Magic = fields[1];
        EffectId = fields[2];

        ExtraFields = fields.Skip(3).ToArray();
    }
}