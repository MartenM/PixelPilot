using System.Drawing;
using Google.Protobuf;
using PixelPilot.Client;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Constants;
using PixelPilot.Client.World.Labels;

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
        for (int layer = 2; layer >= 0; layer--)
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

        List<ITextLabel> labels = new List<ITextLabel>();
        foreach (var placedTextLabel in world.GetLabels())
        {
            if (placedTextLabel.Label.Position.X / 16 >= x && placedTextLabel.Label.Position.X / 16 <= x + width)
            {
                if (placedTextLabel.Label.Position.Y / 16 >= x && placedTextLabel.Label.Position.Y / 16 <= x + height)
                {
                    labels.Add(new TextLabel(placedTextLabel.Label));
                }
            }
        }
        
        return new Structure(width, height, new Dictionary<string, string>(), copyEmpty, blocks, labels);
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

    public class WorldDifference
    {
        public required List<IPlacedBlock> Blocks { get; init; }
        public required List<ITextLabel> Labels { get; init; }

        public IEnumerable<IMessage> AsPackets()
        {
            return Blocks.ToChunkedPackets().Concat(Labels.Select(l => l.ToUpsertPacket()));
        }
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
    public static WorldDifference GetDifference(this PixelWorld world, Structure structure, int x = 0, int y = 0, bool translate = true)
    {
        List<IPlacedBlock> difference = new();
        if (structure.Width + x > world.Width || structure.Height + y > world.Height)
            throw new PixelGameException(
                $"Attempted to a get the difference between a structure and world, but the structure goes outside the world! Structure size: {structure.Width}x{structure.Height} World size: {world.Width}x{world.Height}");
        
        foreach (var block in structure.BlocksWithEmpty)
        {
            var worldBlock = world.BlockAt(block.Layer, block.X + x, block.Y + y);
            if (block.Block.Equals(worldBlock)) continue;

            difference.Add(block);
        }
        
        // Get label difference.
        var labelDifference = structure.Labels.Where(structureLabel =>
        {
            // Modify the structure to get the correct comparison.
            var translatedLabel = new TextLabel(structureLabel);
            translatedLabel.Position = new Point(structureLabel.Position.X + (x * 16), structureLabel.Position.Y + (y * 16)); 
            
            // Rather inefficient, go through all labels and check if there is a matching one.
            foreach (var worldLabel in world.GetLabels())
            {
                if (worldLabel.Label.Equals(translatedLabel))
                {
                    return false;
                }
            }

            return true;
        }).ToList();

        // If not translated, just return the stucture differences.
        if (!translate)
        {
            return new WorldDifference()
            {
                Blocks = difference,
                Labels = labelDifference
            };
        }
        
        // Deep copy the blocks and translate them.
        var translatedBlocks = difference.Select(pb => new PlacedBlock(pb.X + x, pb.Y + y, pb.Layer, (IPixelBlock) pb.Block.Clone())).ToList();
        var translatedLabels = labelDifference.Select(label =>
        {
            var translatedLabel = new TextLabel(label);
            translatedLabel.Position = new Point(label.Position.X + (x * 16), label.Position.Y + (y * 16)); 

            return label;
        });

        return new WorldDifference()
        {
            Blocks = translatedBlocks.Cast<IPlacedBlock>().ToList(),
            Labels = translatedLabels.ToList()
        };
    }
    
    public static async Task PasteSafe(this PixelWorld world, Structure structure, PixelPilotClient client, Point pasteLocation, int maxAttempts = 5)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            var difference = world.GetDifference(structure, pasteLocation.X, pasteLocation.Y, true);
            
            // Blocks
            var diffPackets = difference.Blocks.ToChunkedPackets();
            
            // Labels
            diffPackets.AddRange(difference.Labels.Select(l => l.ToUpsertPacket()));
            
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