namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerRemoveEffectPacket : IPixelGamePlayerPacket
{
    public PlayerRemoveEffectPacket(int id, int effectId)
    {
        PlayerId = id;
        EffectId = effectId;
    }

    public int PlayerId { get; }
    public int EffectId { get; }
}