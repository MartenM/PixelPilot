using System.Text.Json;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;

namespace PixelPilot.Structures.Converters.PilotSimple;

public static class PilotSimpleSerializer
{
    public static Structure ToStructure(string rawData)
    {
        var options = new JsonSerializerOptions()
        {
            Converters =
            {
                new JsonBlockListConverter()
            }
        };
            
        var pilotStruct = JsonSerializer.Deserialize<PilotSimpleStructure>(rawData, options);
        if (pilotStruct == null)
        {
            throw new Exception("Something went wrong. Couldn't load structure from file.");
        }
        
        // Manual migrations for now..
        foreach (var block in pilotStruct.Blocks)
        {
            var flexBlock = block.Block as FlexBlock;
            if (flexBlock == null) continue;
            
            
            if (flexBlock.Block.ToString().StartsWith("BorderGlow"))
            {
                flexBlock.Fields.Add("color", (uint) 16777215);
            }
            if (flexBlock.Block.ToString().StartsWith("BorderOutline"))
            {
                flexBlock.Fields.Add("color", (uint) 16777215);
            }
            if (flexBlock.Block.ToString().Equals("PortalWorld"))
            {
                // This is kinda 'drastic'. But due to permission issues portals don't properly place.
                // For now their placement is just not supported.
                flexBlock.Fields.Remove("spawn_id");
                flexBlock.Fields.Add("spawn_id", "");
            }
        }
        
        return pilotStruct.ToStructure();
    }
}