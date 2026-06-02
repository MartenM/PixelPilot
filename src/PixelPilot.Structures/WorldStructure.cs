using System.Text.Json;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Labels;
using PixelPilot.Client.World.Meta;
using PixelPilot.Structures.Converters.Pilot2;

namespace PixelPilot.Structures;

/// <summary>
/// An extension of the <see cref="Structure"/> that also saves world settings.
/// </summary>
public class WorldStructure : Structure
{
    public const string WorldSettingsKey = "WorldSettings";
    
    private static JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        Converters = { new ColorConverter() }
    };
    
    public WorldStructure(int width, int height, Dictionary<string, string> meta, bool containsEmpty, List<IPlacedBlock> blocks, List<ITextLabel> labels) : base(width, height, meta, containsEmpty, blocks, labels)
    {
        
    }

    public WorldStructure(Structure structure) : base(structure.Width, structure.Height, structure.Meta, structure.ContainsEmpty, structure.Blocks, structure.Labels)
    {
        
    }

    public PixelWorldSettings? WorldSettings
    {
        get
        {
            if (Meta.TryGetValue(WorldSettingsKey, out var worldSettingsData))
            {
                
                return JsonSerializer.Deserialize<PixelWorldSettings>(worldSettingsData, Options);
            }

            return null;
        }
        set => Meta[WorldSettingsKey] = JsonSerializer.Serialize(value, options: Options);
    }
}