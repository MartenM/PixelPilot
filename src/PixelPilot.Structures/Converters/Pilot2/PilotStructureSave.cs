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
    public List<PalletMapping> BlockPallet { get; set; } = new();
    public List<PalletReference> BlockReferences { get; set; } = new();
}