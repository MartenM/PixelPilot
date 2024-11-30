using System.Text.Json.Serialization;

namespace PixelPilot.Client;

public class JoinData
{
    [JsonPropertyName("world_title")]
    public string WorldTitle { get; set; }
    
    [JsonPropertyName("world_width")]
    public int? WorldWidth { get; set; }
    
    [JsonPropertyName("world_height")]
    public int? WorldHeight { get; set; }
}