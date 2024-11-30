namespace PixelPilot.Client.Messages;

/// <summary>
/// Special interface for packets that are triggered by or indicate
/// a status change to a player.
/// </summary>
public interface IPixelGamePlayerPacket : IPixelGamePacket
{
    public int PlayerId { get; }
}