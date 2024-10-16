using System.Drawing;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelHttpClient;

namespace PixelPilot.Structures.Extensions;

public static class PlacedBlockListExtensions
{
    private const int MaxChunkBlockCount = 250;
    
    /// <summary>
    /// Group blocks that are the same into a single packet that can be send.
    /// </summary>
    /// <param name="blocks">The blocks</param>
    /// <returns>Packets to be send by the client.</returns>
    public static List<IPixelGamePacketOut> ToChunkedPackets(this IEnumerable<IPlacedBlock> blocks)
    {
        var result = new List<IPixelGamePacketOut>();
        
        // Group the blocks per type/id
        var groupedBlocks = blocks.GroupBy(block => new {Block = block.Block, Layer = block.Layer});
        
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