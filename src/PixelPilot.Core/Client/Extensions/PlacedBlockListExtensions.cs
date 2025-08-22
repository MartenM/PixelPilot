using System.Drawing;
using Google.Protobuf;
using PixelPilot.Api;
using PixelPilot.Client.World.Blocks.Placed;

namespace PixelPilot.Client.Extensions;

public static class PlacedBlockListExtensions
{
    private const int MaxChunkBlockCount = 250;
    
    /// <summary>
    /// Group blocks that are the same into a single packet that can be send.
    /// </summary>
    /// <param name="blocks">The blocks</param>
    /// <returns>Packets to be send by the client.</returns>
    public static List<IMessage> ToChunkedPackets(this IEnumerable<IPlacedBlock> blocks)
    {
        var result = new List<IMessage>();
        
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
    public static IMessage ToChunkedPacket(this List<IPlacedBlock> blocks)
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
}