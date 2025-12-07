using System.Text.Json;
using PixelPilot.Api;

namespace PixelPilot.Structures.Converters.PilotSimple;

public static class PilotSaveSerializer
{
    [Obsolete("No longer supported. Please use the newer safe format.")]
    public static string Serialize(Structure structure)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Obsolete, please migrate to the latest version A.S.P.")]
    public static Structure Deserialize(PixelApiClient apiClient, string rawData)
    {
        var options = new JsonSerializerOptions()
        {
            Converters =
            {
                new JsonBlockListConverter(apiClient)
            }
        };

        var pilotStruct = JsonSerializer.Deserialize<PilotSimpleStructure>(rawData, options);
        return pilotStruct?.ToStructure() ?? throw new Exception("Something went wrong. Couldn't load structure from file.");
    }
}