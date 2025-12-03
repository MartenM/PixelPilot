using PixelPilot.Client.World.Constants;
using PixelPilot.Structures.Converters.Changes;

namespace PixelPilot.Structures.Converters.PilotSimple;

public class MappedBlockData
{
    public MappedBlockData()
    {
        
    }
    public MappedBlockData(List<PixelBlock> mapping, List<string> blockData)
    {
        Version = VersionManager.CurrentVersion();
        Mapping = mapping.Select(p => p.ToString()).ToList();
        BlockData = blockData;
    }
    
    public int Version { get; set; }
    public List<string> Mapping { get; set; } = new();
    public List<string> BlockData { get; set; } = new();
}