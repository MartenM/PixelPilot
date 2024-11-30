namespace PixelPilot.Client.Messages.Received;

public class PlayerPrivateMessagePacket : IPixelGamePlayerPacket
{
    public int PlayerId { get; }
    public string Messsage { get; }
    
    public PlayerPrivateMessagePacket(int id, string messsage)
    {
        PlayerId = id;
        Messsage = messsage;
    }
}