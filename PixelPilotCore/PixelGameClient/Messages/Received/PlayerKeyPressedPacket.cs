namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerKeyPressedPacket : IPixelGamePacket
{
    public PlayerKeyPressedPacket(byte key)
    {
        Key = key;
    }

    public int Key { get; }
}