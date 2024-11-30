using System.Text.Json.Serialization;

namespace PixelPilot.Api.Responses;

public class JoinKeyResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
}