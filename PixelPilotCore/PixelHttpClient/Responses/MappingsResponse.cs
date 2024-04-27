namespace PixelPilot.PixelHttpClient.Responses;

public class MappingsResponse
{
    private Dictionary<String, int> mappings { get; }

    public MappingsResponse(Dictionary<string, int> mappings)
    {
        this.mappings = mappings;
    }
}