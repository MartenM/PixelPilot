using System.Text.Json.Serialization;

namespace PixelPilot.Api.Responses.Auth;

public class AuthSuccessResponse : IAuthResponse
{
    [JsonPropertyName("record")]
    public PlayerData Data { get; set; } = null!;
    
    [JsonPropertyName("token")]
    public string Token { get; set; }  = null!;
}