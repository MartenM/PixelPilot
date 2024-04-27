namespace PixelPilot.PixelGameClient.Messages.Exceptions;

public class PacketConstructorException : PixelException
{
    public List<dynamic> ReceivedFields { get; }
    public Type PacketType { get; }
    
    public PacketConstructorException(List<dynamic> receivedFields, Type packetType) : base($"Could not find the correct constructor for {packetType}")
    {
        ReceivedFields = receivedFields;
        PacketType = packetType;
    }
}