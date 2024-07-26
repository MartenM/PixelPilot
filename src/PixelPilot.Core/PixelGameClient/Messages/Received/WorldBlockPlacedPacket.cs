using System.Drawing;
using PixelPilot.PixelGameClient.World.Blocks.Placed;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class WorldBlockPlacedPacket : IDynamicConstructedPacket
{
    // Basic information that every block placement has.
    public int PlayerId { get; }

    public Point[] Positions { get; }
    public int Layer { get; }
    public int BlockId { get; }
    
    // Extra data depending on what block type has been placed.
    public dynamic[] ExtraFields { get; }

    public WorldBlockPlacedPacket(List<dynamic> fields)
    {
        PlayerId = fields[0];

        var rawPositions = (byte[]) fields[1];
        Positions = new Point[rawPositions.Length / 4];
        for (int i = 0; i < Positions.Length; i++)
        {
            var iX = i * 4;
            var x = rawPositions[iX] | (rawPositions[iX + 1] << 8);
            var y = rawPositions[iX + 2] | (rawPositions[iX + 3] << 8);
            Positions[i] = new Point(x, y);
        }
        
        Layer = fields[2];
        BlockId = fields[3];

        ExtraFields = fields.Skip(4).ToArray();
    }
}