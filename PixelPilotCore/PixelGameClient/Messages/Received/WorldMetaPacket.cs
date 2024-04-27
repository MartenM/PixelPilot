namespace PixelPilot.PixelGameClient.Messages.Received;

public class WorldMetaPacket : IPixelGamePacket
{
    public WorldMetaPacket(string name, int plays, string owner)
    {
        Name = name;
        Plays = plays;
        Owner = owner;
    }

    public string Name { get; }
    public int Plays { get; }
    public string Owner { get; }
}