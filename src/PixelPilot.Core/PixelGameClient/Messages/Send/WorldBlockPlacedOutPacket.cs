using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class WorldBlockPlacedOutPacket : ReflectivePixelOutPacket
{
    // Basic information that every block placement has.
    public byte[] RawPositions { get; }
    public int Layer { get; }
    public int BlockId { get; }
    
    // Extra data depending on what block type has been placed.
    public dynamic[]? ExtraData { get; } = null;
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId) : base(WorldMessageType.WorldBlockPlaced)
    {
        RawPositions = new byte[4];
        RawPositions[0] = (byte) (x & 0xFF);
        RawPositions[1] = (byte) ((x >> 8) & 0xFF);
        RawPositions[2] = (byte) (y & 0xFF);
        RawPositions[3] = (byte) ((y >> 8) & 0xFF);
        
        Layer = layer;
        BlockId = blockId;
    }
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId, dynamic[] extraData) : base(WorldMessageType.WorldBlockPlaced)
    {
        RawPositions = new byte[4];
        RawPositions[0] = (byte) (x & 0xFF);
        RawPositions[1] = (byte) ((x >> 8) & 0xFF);
        RawPositions[2] = (byte) (y & 0xFF);
        RawPositions[3] = (byte) ((y >> 8) & 0xFF);
        
        
        Layer = layer;
        
        BlockId = blockId;
        ExtraData = extraData;
    }
    
    protected override List<dynamic> GetFields()
    {
        return GetFields(true);
    }
}