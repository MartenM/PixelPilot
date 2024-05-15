namespace PixelPilot.PixelGameClient.Messages.Received;

public class KeyPressedPacket : IPixelGamePacket
{
    public KeyPressedPacket(byte key)
    {
        Key = key;
    }

    public int Key { get; }
}