namespace PixelPilot.PixelGameClient.Messages;

/// <summary>
/// Constructor that enforces the contract that the packet implements a `List of dynamic type` constructor.
/// This is used for when a packet cannot simply be constructed using a constructor because the packet length may vary. 
/// </summary>
public interface IDynamicConstructedPacket : IPixelGamePacket
{
    
}