using System.Text.Json.Serialization;

namespace PixelPilot.PixelHttpClient.Responses;

public class PlayerData
{
    [JsonPropertyName("admin")]
    public bool Admin { get; set; }
    
    [JsonPropertyName("banned")]
    public bool Banned { get; set; }
    
    [JsonPropertyName("face")]
    public int Face { get; set; }

    [JsonPropertyName("username")] public string Username { get; set; } = null!;
}