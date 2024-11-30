using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Exceptions;

public class PacketTypeNotFoundException : PixelException
{
    public WorldMessageType WorldMessageType { get; }
    
    public PacketTypeNotFoundException(WorldMessageType worldMessageType) : base($"No packet implementation was found for {worldMessageType}.")
    {
        WorldMessageType = worldMessageType;
    }
}