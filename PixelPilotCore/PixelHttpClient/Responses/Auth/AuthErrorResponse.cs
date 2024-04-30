using System.Text.Json.Serialization;

namespace PixelPilot.PixelHttpClient.Responses.Auth;

public class AuthErrorResponse : IAuthResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }  = null!;
}