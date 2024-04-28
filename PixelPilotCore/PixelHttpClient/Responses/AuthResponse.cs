using System.Text.Json.Serialization;

namespace PixelPilot.PixelHttpClient.Responses;

public interface IAuthResponse
{
    
}

public class AuthSuccessResponse : IAuthResponse
{
    [JsonPropertyName("record")]
    public PlayerData PlayerData { get; set; }
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
}

public class AuthErrorResponse : IAuthResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class PlayerData
{
    [JsonPropertyName("admin")]
    public bool Admin { get; set; }
    
    [JsonPropertyName("banned")]
    public bool Banned { get; set; }
    
    [JsonPropertyName("face")]
    public int Face { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
}