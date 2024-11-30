using System.Drawing;
using PixelPilot.Client;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Constants;

namespace PixelPilot.Structures.Extensions;

public static class WorldExtensions
{
    /// <summary>
    /// Get a structure from the world.
    /// </summary>
    /// <param name="world"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="copyEmpty"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Get a structure from the world between two points.
    /// </summary>
    /// <param name="world"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="copyEmpty"></param>
    /// <returns></returns>
    public static Structure GetStructure(this PixelWorld world, Point p1, Point p2, bool copyEmpty = false)
    {
        var topleft = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
        
        // Calculate width and height
        var width = Math.Abs(p1.X - p2.X);
        var height = Math.Abs(p1.Y - p2.Y);

        return GetStructure(world, topleft.X, topleft.Y, width + 1, height + 1, copyEmpty);
    }

    /// <summary>
    /// Get the difference between the world and structure at a specified place in the world.
    /// The returned list will be translated towards the origin point (unless disabled using the parameters) by copying the original blocks.
    /// If not translated the original references will be kept.
    /// </summary>
    /// <param name="world">The from which to check if blocks are already placed</param>
    /// <param name="structure">The structure to be placed.</param>
    /// <param name="x">The X location (Top Left)</param>
    /// <param name="y">The Y location (Top Left)</param>
    /// <param name="translate">If the output should be translated to the differences in the world.</param>
    /// <returns></returns>
    /// <exception cref="PixelGameException"></exception>
    public static List<IPlacedBlock> GetDifference(this PixelWorld world, Structure structure, int x = 0, int y = 0, bool translate = true)
    {
        List<IPlacedBlock> difference = new();
        if (structure.Width + x > world.Width || structure.Height + y > world.Height)
            throw new PixelGameException(
                "Attempted to a get the difference between a structure and world, but the structure goes outside the world!");
        
        foreach (var block in structure.BlocksWithEmpty)
        {
            var worldBlock = world.BlockAt(block.Layer, block.X + x, block.Y + y);
            if (block.Block.Equals(worldBlock)) continue;

            difference.Add(block);
        }

        // If not translated, just return the stucture differences.
        if (!translate) return difference;
        
        // Deep copy the blocks and translate them.
        var translated = difference.Select(pb => new PlacedBlock(pb.X + x, pb.Y + y, pb.Layer, (IPixelBlock) pb.Block.Clone())).ToList();
        return translated.Cast<IPlacedBlock>().ToList();
    }
    
    public static async Task PasteSafe(this PixelWorld world, Structure structure, PixelPilotClient client, Point pasteLocation, int maxAttempts = 5)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            var diffPackets = world
                .GetDifference(structure, pasteLocation.X, pasteLocation.Y, true)
                .ToChunkedPackets();
            
            // If no diff, return.
            if (diffPackets.Count == 0) return;
            
            // Send packets. Assumes max ping 250.
            client.SendRange(diffPackets);
            await Task.Delay(250 + diffPackets.Count * 5);
            
            attempts++;
        }
        
        if (attempts >= maxAttempts) throw new Exception("Too many attempts to paste the structure!");
    }
}