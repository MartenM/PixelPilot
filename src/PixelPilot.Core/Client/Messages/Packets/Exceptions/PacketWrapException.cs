namespace PixelPilot.Client.Messages.Packets.Exceptions;

public class PacketWrapException : Exception
{
    private string PacketToWrap { get; }
    private string WrapperPacket { get; }

    public PacketWrapException(string packetToWrap, string wrapperPacket) : base($"Could not wrap {packetToWrap} into {wrapperPacket}.")
    {
        this.PacketToWrap = packetToWrap;
        this.WrapperPacket = wrapperPacket;
    }
}