using PixelPilot.Client.Messages.Send;

namespace PixelPilot.Client.Messages.Received;

public class PlayerChatPacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public PlayerChatPacket(int id, string message)
    {
        PlayerId = id;
        Message = message;
    }

    public int PlayerId { get; }
    public string Message { get; }
    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerChatOutPacket(Message);
    }
}