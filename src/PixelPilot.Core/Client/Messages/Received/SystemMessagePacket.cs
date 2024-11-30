namespace PixelPilot.Client.Messages.Received;

public class SystemMessagePacket : IPixelGamePacket
{
    public SystemMessagePacket(string prefix, string message, bool kicked)
    {
        Prefix = prefix;
        Message = message;
        Kicked = kicked;
    }

    public string Prefix { get; }
    public string Message { get; }
    public bool Kicked { get; }
}