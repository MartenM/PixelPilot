namespace PixelPilot.PixelGameClient.Messages.Received;

public class WorldBlockPlacedPacket : IPixelGamePacket
{
    // Basic information that every block placement has.
    public int PlayerId { get; }
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }
    public int BlockId { get; }
    
    // Extra data depending on what block type has been placed.
    public int? ExtraInt1 { get; }
    public int? ExtraInt2 { get; }
    public int? ExtraInt3 { get; }

    public bool? ExtraBool { get; }
    
    public byte? ExtraByte { get; }

    public WorldBlockPlacedPacket(int playerId, int x, int y, int layer, int blockId)
    {
        PlayerId = playerId;
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
    }
    
    public WorldBlockPlacedPacket(int playerId, int x, int y, int layer, int blockId, int extraInt1)
    {
        PlayerId = playerId;
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = extraInt1;
    }
    
    public WorldBlockPlacedPacket(int playerId, int x, int y, int layer, int blockId, int extraInt1, byte extraByte)
    {
        PlayerId = playerId;
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = extraInt1;
        ExtraByte = extraByte;
    }
    
    public WorldBlockPlacedPacket(int playerId, int x, int y, int layer, int blockId, byte extraByte)
    {
        PlayerId = playerId;
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraByte = extraByte;
    }
    
    public WorldBlockPlacedPacket(int playerId, int x, int y, int layer, int blockId, int extraInt1, int extraInt2, int extraInt3)
    {
        PlayerId = playerId;
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = extraInt1;
        ExtraInt2 = extraInt2;
        ExtraInt3 = extraInt3;
    }
    
    public WorldBlockPlacedPacket(int playerId, int x, int y, int layer, int blockId, int extraInt1, bool extraBool)
    {
        PlayerId = playerId;
        X = x;
        Y = y;
        Layer = layer;
        BlockId = blockId;
        ExtraInt1 = extraInt1;
        ExtraBool = extraBool;
    }
}