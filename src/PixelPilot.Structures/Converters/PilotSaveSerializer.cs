using System.Text;
using System.Text.Json;
using PixelPilot.Api;
using PixelPilot.Structures.Converters.Pilot2;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters;

/// <summary>
/// Top level helper class that is used to parse from the various structure schemas.
/// </summary>
public static class PilotSaveSerializer
{
    /// <summary>
    /// Serializes a structure to the newest save format.
    /// </summary>
    /// <param name="structure"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static string Serialize(Structure structure)
    {
        return Pilot2Serializer.ToJson(structure);
    }
    
    public static Structure Deserialize(string rawData)
    {
        // Multiple safe format, determine which one is being used.
        var version = DetermineVersion(rawData);
        if (version == null)
        {
            throw new PixelApiException("The data you provided is not valid. Version could not be determined.");
        }

        if (version < 20)
        {
            // Old legacy format.
            return PilotSimpleSerializer.ToStructure(rawData);
        }

        if (version >= 20)
        {
            // Newer format
            return Pilot2Serializer.ToStructure(rawData);
        }
        
        throw new PixelApiException("Couldn't load structure from file. Version could not be determined.");
    }

    

    private static int? DetermineVersion(string rawData)
    {
        byte[] utf8 = Encoding.UTF8.GetBytes(rawData);
        var reader = new Utf8JsonReader(utf8);
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName &&
                reader.ValueTextEquals("Version"))
            {
                reader.Read();                     // move to value
                return reader.GetInt32();
            }
        }

        return null;
    }
}