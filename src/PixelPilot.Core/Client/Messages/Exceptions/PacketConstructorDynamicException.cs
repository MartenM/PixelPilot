namespace PixelPilot.Client.Messages.Exceptions;

public class PacketConstructorDynamicException(string msg)
    : PixelException($"A check failed during the use of a dynamic constructor. Check: {msg}");