using System.Text.Json;

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
        return pilotStruct?.ToStructure() ?? throw new Exception("Something went wrong. Couldn't load structure from file.");
    }
}