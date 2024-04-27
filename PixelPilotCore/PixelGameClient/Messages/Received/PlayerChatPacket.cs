namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerChatPacket : IPixelGamePacket
{
    public PlayerChatPacket(int id, string message)
    {
        Id = id;
        Message = message;
    }

    public int Id { get; }
    public string Message { get; }
}