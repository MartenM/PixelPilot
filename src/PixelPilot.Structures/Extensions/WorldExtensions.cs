using System.Drawing;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.World;
using PixelPilot.PixelGameClient.World.Blocks;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelGameClient.World.Constants;
using PixelPilot.PixelHttpClient;

namespace PixelPilot.Structures.Extensions;

public static class WorldExtensions
{
    public const int MaxChunkBlockCount = 250;
    
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

    /// <summary>
    /// Group blocks that are the same into a single packet that can be send.
    /// </summary>
    /// <param name="blocks">The blocks</param>
    /// <returns>Packets to be send by the client.</returns>
    public static List<IPixelGamePacketOut> ToChunkedPackets(this IEnumerable<IPlacedBlock> blocks)
    {
        var result = new List<IPixelGamePacketOut>();
        
        // Group the blocks per type/id
        var groupedBlocks = blocks.GroupBy(block => block.Block);
        
        // Now create the chunks of 250 blocks.
        foreach (var rawGroup in groupedBlocks)
        {
            var typeBlocks = rawGroup.ToList();
            for (int i = 0; i < typeBlocks.Count; i += MaxChunkBlockCount)
            {
                var chunk = typeBlocks.Skip(i).Take(MaxChunkBlockCount).ToList();
                result.Add(chunk.ToChunkedPacket());
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a packet out of a blocks that are all the same.
    /// </summary>
    /// <param name="blocks">The blocks</param>
    /// <returns>A packet</returns>
    /// <exception cref="PixelApiException">When the requirements are not met</exception>
    public static IPixelGamePacketOut ToChunkedPacket(this List<IPlacedBlock> blocks)
    {
        var blockData = blocks.First().Block;
        var layer = blocks.First().Layer;
        
        if (blocks.Count > MaxChunkBlockCount)
            throw new PixelApiException("Cannot convert more than 250 blocks into a chunk.");

        if (!blocks.All(block => block.Block.Equals(blockData)))
            throw new PixelApiException("All blocks need to be the same for them to be chunked together.");

        var positions = blocks.Select(b => new Point(b.X, b.Y)).ToList();
        return blockData.AsPacketOut(positions, layer);
    }
    
    public static async Task PasteInOrder(this List<IPlacedBlock> blocks, PixelPilotClient client, Point origin, int delay = 0)
    {
        foreach (var block in blocks)
        {
            client.Send(block.Block.AsPacketOut(block.X + origin.X, block.Y + origin.Y, block.Layer));
            if (delay > 0) await Task.Delay(delay);
        }
    }
    
    public static async Task PasteShuffled(this List<IPlacedBlock> blocks, PixelPilotClient client, Point origin, int delay = 0)
    {
        var shuffeled = new List<IPlacedBlock>(blocks);
        Shuffle(shuffeled);
        foreach (var block in shuffeled)
        {
            client.Send(block.Block.AsPacketOut(block.X + origin.X, block.Y + origin.Y, block.Layer));
            if (delay > 0) await Task.Delay(delay);
        }
    }
    
    private static void Shuffle<T>(this IList<T> list)  
    {  
        var rng = new Random();
        var n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}