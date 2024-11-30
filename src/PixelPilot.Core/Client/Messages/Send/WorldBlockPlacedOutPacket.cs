using System.Drawing;
using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.Messages.Send;

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
    
    public WorldBlockPlacedOutPacket(int x, int y, int layer, int blockId, dynamic[] extraData) : this(x, y, layer, blockId)
    {
        ExtraData = extraData;
    }
    
    public WorldBlockPlacedOutPacket(List<Point> positions, int layer, int blockId) : base(WorldMessageType.WorldBlockPlaced)
    {
        RawPositions = new byte[positions.Count * 4];
        for (int i = 0; i < positions.Count; i++)
        {
            var rootIndex = i * 4;
            var pos = positions[i];
            RawPositions[rootIndex + 0] = (byte) (pos.X & 0xFF);
            RawPositions[rootIndex + 1] = (byte) ((pos.X >> 8) & 0xFF);
            RawPositions[rootIndex + 2] = (byte) (pos.Y & 0xFF);
            RawPositions[rootIndex + 3] = (byte) ((pos.Y >> 8) & 0xFF);
        }
        
        Layer = layer;
        BlockId = blockId;
    }
    
    public WorldBlockPlacedOutPacket(List<Point> positions, int layer, int blockId, dynamic[] extraData) : this(positions, layer, blockId)
    {
        ExtraData = extraData;
    }

    protected override List<dynamic> GetFields()
    {
        return GetFields(true);
    }
}