namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerChatPacket : IPixelGamePlayerPacket
{
    public PlayerChatPacket(int id, string message)
    {
        PlayerId = id;
        Message = message;
    }

    public int PlayerId { get; }
    public string Message { get; }
}