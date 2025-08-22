using System.Drawing;
using Google.Protobuf;
using PixelPilot.Api;
using PixelPilot.Client;
using PixelPilot.Client.Messages;
using PixelPilot.Client.World.Blocks.Placed;

namespace PixelPilot.Structures.Extensions;

public static class ExtraPlacedBlockListExtensions
{
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