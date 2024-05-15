using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Exceptions;

public class PacketTypeNotFoundException : PixelException
{
    public WorldMessageType WorldMessageType { get; }
    
    public PacketTypeNotFoundException(WorldMessageType worldMessageType) : base($"No packet implementation was found for {worldMessageType}.")
    {
        WorldMessageType = worldMessageType;
    }
}