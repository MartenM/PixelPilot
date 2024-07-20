namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerEffectPacket : IPixelGamePlayerPacket, IDynamicConstructedPacket
{
    public int PlayerId { get; }
    
    public int EffectId { get; }
    
    public dynamic[] ExtraFields { get; }
    
    public PlayerEffectPacket(List<dynamic> fields)
    {
        PlayerId = fields[0];
        EffectId = fields[1];
        
        ExtraFields = fields.Skip(2).ToArray();
    }
}