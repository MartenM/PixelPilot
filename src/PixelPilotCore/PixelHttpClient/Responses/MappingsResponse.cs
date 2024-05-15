using System.Globalization;

namespace PixelPilot.PixelHttpClient.Responses;

public class MappingsResponse
{
    public Dictionary<String, int> mappings { get; }

    public MappingsResponse(Dictionary<string, int> mappings)
    {
        this.mappings = mappings;
    }

    /// <summary>
    /// Method used to quickly generate a ENUM like mapping from the entries.
    /// </summary>
    /// <returns></returns>
    public List<string> AsEnumEntries()
    {
        var list = new List<string>();
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        foreach (var pair in mappings)
        {
            var words = pair.Key.Split("_");
            list.Add($"{String.Join(null, words.Select(w => textInfo.ToTitleCase(w)))} = {pair.Value}");
        }

        return list;
    }
}