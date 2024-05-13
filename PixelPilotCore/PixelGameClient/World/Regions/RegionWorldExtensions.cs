using PixelPilot.PixelGameClient.World.Blocks;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.PixelGameClient.World.Regions;

/// <summary>
/// Extensions for getting regions from a world.
/// </summary>
public static class RegionWorldExtensions
{
    public static Region GetRegion(this PixelWorld world, int x, int y, int width, int height)
    {
        var regionBlocks = new IPixelBlock[2, width, height];
        for (int layer = 0; layer < 2; layer++)
        {
            for (int dx = 0; dx < width; dx++)
            {
                for (int dy = 0; dy < height; dy++)
                {
                    regionBlocks[layer, dx, dy] = world.BlockAt((WorldLayer)layer, x + dx, y +dy);
                }
            }
        } 
        
        return new Region($"Region-{x}-{y}-{width}-{height}", "PixelPilot", regionBlocks);
    }

    public static void PasteRegion(this PixelWorld world, Region region, int x, int y)
    {
        
    }
}