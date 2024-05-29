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
            for (int dx = 0; dx < width; dx++)
            {
                for (int dy = 0; dy < height; dy++)
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

        return GetStructure(world, topleft.X, topleft.Y, width + 1, height + 1, copyEmpty);
    }

    public static List<IPlacedBlock> GetDifference(this PixelWorld world, Structure structure, int x = 0, int y = 0)
    {
        List<IPlacedBlock> difference = new();
        if (structure.Width + x > world.Width || structure.Height + y > world.Height)
            throw new PixelGameException(
                "Attempted to a get the difference between a structure and world, but the structure goes outside the world!");
        
        foreach (var block in structure.BlocksWithEmpty)
        {
            var worldBlock = world.BlockAt(block.Layer, block.X + x, block.Y + x);
            if (block.Block.Equals(worldBlock)) continue;

            difference.Add(block);
        }

        return difference;
    }
    public static async Task PasteInOrder(this List<IPlacedBlock> blocks, PixelPilotClient client, Point origin, int delay)
    {
        foreach (var block in blocks)
        {
            client.Send(block.Block.AsPacketOut(block.X + origin.X, block.Y + origin.Y, block.Layer));
            await Task.Delay(delay);
        }
    }
    
    public static async Task PasteShuffled(this List<IPlacedBlock> blocks, PixelPilotClient client, Point origin, int delay)
    {
        var shuffeled = new List<IPlacedBlock>(blocks);
        Shuffle(shuffeled);
        foreach (var block in shuffeled)
        {
            client.Send(block.Block.AsPacketOut(block.X + origin.X, block.Y + origin.Y, block.Layer));
            await Task.Delay(delay);
        }
    }
    
    private static void Shuffle<T>(this IList<T> list)  
    {  
        var rng = new Random();
        var n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}