using System.Text.Json;

namespace PixelPilot.Structures.Converters.PilotSimple;

public static class PilotSaveSerializer
{
    public static string Serialize(Structure structure)
    {
        var options = new JsonSerializerOptions()
        {
            Converters =
            {
                new JsonBlockListConverter()
            }
        };

        var pilotStruct = PilotSimpleStructure.FromStructure(structure);
        var json = JsonSerializer.Serialize(pilotStruct, options);
        return json;
    }

    public static Structure Deserialize(string rawData)
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