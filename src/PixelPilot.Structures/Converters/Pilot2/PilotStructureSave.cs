namespace PixelPilot.Structures.Converters.Pilot2;

/// <summary>
/// DTO Used to hold structure information that can be saved to JSON.
/// </summary>
public class PilotStructureSave
{
    // Save version used to indicate what version this safe is using.
    public int Version { get; set; } = 20;
    
    public int Width { get; set; }
    public int Height { get; set; }
    public Dictionary<string, string> Meta { get; set; } = new();

    public int BlocksVersion { get; set; }
    
    /// <summary>
    /// Stores the 'block data' once such that it can be referenced in the blockReferences.
    /// </summary>
    public List<PalletMapping> BlockPallet { get; set; } = new();
    
    /// <summary>
    /// Stores what blocks are on a certain position and layer.
    /// </summary>
    public List<PalletReference> BlockReferences { get; set; } = new();
}