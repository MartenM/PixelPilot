using System.Text.Json.Serialization;

namespace PixelPilot.PixelHttpClient.Responses;

public class JoinKeyResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
}