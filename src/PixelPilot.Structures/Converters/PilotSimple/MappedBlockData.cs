using PixelPilot.Client.World.Constants;

namespace PixelPilot.Structures.Converters.PilotSimple;

public class MappedBlockData
{
    public MappedBlockData()
    {
        
    }
    public MappedBlockData(List<PixelBlock> mapping, List<string> blockData)
    {
        Version = 3;
        Mapping = mapping.Select(p => p.ToString()).ToList();
        BlockData = blockData;
    }
    
    public int Version { get; set; }
    public List<string> Mapping { get; set; } = new();
    public List<string> BlockData { get; set; } = new();
}