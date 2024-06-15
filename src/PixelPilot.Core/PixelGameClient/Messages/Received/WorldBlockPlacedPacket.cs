namespace PixelPilot.PixelGameClient.Messages.Received;

public class WorldBlockPlacedPacket : IDynamicConstructedPacket
{
    // Basic information that every block placement has.
    public int PlayerId { get; }
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    public int BlockId { get; }
    
    // Extra data depending on what block type has been placed.
    public dynamic[] ExtraFields { get; }

    public WorldBlockPlacedPacket(List<dynamic> fields)
    {
        PlayerId = fields[0];
        X = fields[1];
        Y = fields[2];
        Layer = fields[3];
        BlockId = fields[4];

        ExtraFields = fields.Skip(5).ToArray();
    }
}