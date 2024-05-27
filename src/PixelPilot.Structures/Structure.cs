using PixelPilot.PixelGameClient.World.Blocks.Placed;

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
}