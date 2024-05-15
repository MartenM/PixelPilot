namespace PixelPilot.PixelGameClient.Messages.Received;

public class SystemMessagePacket : IPixelGamePacket
{
    public SystemMessagePacket(string prefix, string message, bool magic)
    {
        Prefix = prefix;
        Message = message;
        Magic = magic;
    }

    public string Prefix { get; }
    public string Message { get; }
    public bool Magic { get; }
}