namespace PixelPilot.Client.Messages.Received;

public class WorldReloadedPacket : IPixelGamePacket
{
    public WorldReloadedPacket(byte[] worldData)
    {
        WorldData = worldData;
    }

    public byte[] WorldData { get; }
}