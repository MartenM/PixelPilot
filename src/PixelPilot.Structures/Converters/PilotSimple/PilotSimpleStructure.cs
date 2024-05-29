using PixelPilot.PixelGameClient.World.Blocks.Placed;

namespace PixelPilot.Structures.Converters.PilotSimple;

/// <summary>
/// Used to save a structure
/// </summary>
public class PilotSimpleStructure
{
    public int Version { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Dictionary<string, string> Meta { get; set; } = null!;
    public bool ContainsEmpty { get; set; }
    public List<IPlacedBlock> Blocks { get; set; } = new();

    public PilotSimpleStructure() {}
    private PilotSimpleStructure(int width, int height, Dictionary<string, string> meta, bool containsEmpty, List<IPlacedBlock> blocks)
    {
        Version = 1;
        Width = width;
        Height = height;
        Meta = meta;
        ContainsEmpty = containsEmpty;
        Blocks = blocks;
    }

    public static PilotSimpleStructure FromStructure(Structure structure)
    {
        return new PilotSimpleStructure(structure.Width, structure.Height, structure.Meta, structure.ContainsEmpty, structure.Blocks);
    }

    public Structure ToStructure()
    {
        return new Structure(Width, Height, Meta, ContainsEmpty, Blocks);
    }
}