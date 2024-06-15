using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class WorldBlockPlacedOutPacket : ReflectivePixelOutPacket
{
    // Basic information that every block placement has.
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    public int BlockId { get; }
    
    // Extra data depending on what block type has been placed.
    public dynamic[]? ExtraData { get; } = null;
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId) : base(WorldMessageType.WorldBlockPlaced)
    {
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
    }
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId, dynamic[] extraData) : base(WorldMessageType.WorldBlockPlaced)
    {
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraData = extraData;
    }
    
    protected override List<dynamic> GetFields()
    {
        return GetFields(true);
    }
}