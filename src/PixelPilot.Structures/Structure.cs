using PixelPilot.PixelGameClient.World.Blocks;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.Structures;

public class Structure
{
    public Structure(int width, int height, Dictionary<string, string> meta, bool containsEmpty, List<IPlacedBlock> blocks)
    {
        Width = width;
        Height = height;
        Meta = meta;
        ContainsEmpty = containsEmpty;
        Blocks = blocks;
    }
    public int Width { get; }
    public int Height { get; }
    public Dictionary<string, string> Meta { get; set; }
    public bool ContainsEmpty { get; set; }
    public List<IPlacedBlock> Blocks { get; }

    /// <summary>
    /// Returns a computed list of the structure which includes empty blocks.
    /// </summary>
    public List<IPlacedBlock> BlocksWithEmpty
    {
        get
        {
            if (ContainsEmpty) return Blocks;
            
            // Fill with air blocks at the end.
            var placed = new bool[2, Width, Height];
            var emptyList = new List<IPlacedBlock>();
            
            foreach (var block in Blocks)
            {
                placed[block.Layer, block.X, block.Y] = true;
            }
            
            // Iterate over this 3D array and add places that are still false.
            // Iterate through each dimension of the array
            for (int l = 0; l < 2; l++) // First dimension
            {
                for (int x = 0; x < Width; x++) // Second dimension (Width)
                {
                    for (int y = 0; y < Height; y++) // Third dimension (Height)
                    {
                        if (!placed[l, x, y])
                        {
                            emptyList.Add(new PlacedBlock(x, y, l, new BasicBlock(PixelBlock.Empty)));
                        }
                    }
                }
            }
            
            emptyList.AddRange(Blocks);
            return emptyList;
        }
    }
}