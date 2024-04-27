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
    public int? ExtraInt1 { get; }
    public int? ExtraInt2 { get; }
    public int? ExtraInt3 { get; }

    public bool? ExtraBool { get; }
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId, int? extraInt1, int? extraInt2, int? extraInt3, bool? extraBool) : base(WorldMessageType.WorldBlockPlaced)
    {
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = extraInt1;
        ExtraInt2 = extraInt2;
        ExtraInt3 = extraInt3;
        ExtraBool = extraBool;
    }
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId) : base(WorldMessageType.WorldBlockPlaced)
    {
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = null;
        ExtraInt2 = null;
        ExtraInt3 = null;
        ExtraBool = null;
    }
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId, int? extraInt1) : base(WorldMessageType.WorldBlockPlaced)
    {
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = extraInt1;
        ExtraInt2 = null;
        ExtraInt3 = null;
        ExtraBool = null;
    }

    protected override List<dynamic> GetFields()
    {
        return GetFields(true);
    }
}