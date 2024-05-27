using System.Drawing;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.World;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.Structures.Extensions;

public static class WorldExtensions
{
    public static Structure GetStructure(this PixelWorld world, int x, int y, int width, int height, bool copyEmpty = true)
    {
        List<IPlacedBlock> blocks = new();
        
        // Copy the world based on X Y
        for (int layer = 1; layer >= 0; layer--)
        {
            for (int dx = 0; dx <= width; dx++)
            {
                for (int dy = 0; dy <= height; dy++)
                {
                    var block = world.BlockAt(layer, x + dx, y + dy);
                    if (block.Block == PixelBlock.Empty && !copyEmpty) continue;
                    blocks.Add(new PlacedBlock(dx, dy, layer, block));
                }
            }
        }

        return new Structure(width, height, new Dictionary<string, string>(), copyEmpty, blocks);
    }
    
    public static Structure GetStructure(this PixelWorld world, Point p1, Point p2, bool copyEmpty = false)
    {
        var topleft = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
        
        // Calculate width and height
        var width = Math.Abs(p1.X - p2.X);
        var height = Math.Abs(p1.Y - p2.Y);

        return GetStructure(world, topleft.X, topleft.Y, width, height, copyEmpty);
    }

    public static async Task PasteInOrder(this Structure structure, PixelPilotClient client, Point origin, int delay)
    {
        foreach (var block in structure.Blocks)
        {
            client.Send(block.Block.AsPacketOut(block.X + origin.X, block.Y + origin.Y, block.Layer));
            await Task.Delay(delay);
        }
    }
}