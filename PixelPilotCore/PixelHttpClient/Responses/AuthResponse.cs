using System.Text.Json.Serialization;

namespace PixelPilot.PixelHttpClient.Responses;

public interface IAuthResponse
{
    
}

public class AuthSuccessResponse : IAuthResponse
{
    [JsonPropertyName("record")]
    public PlayerData Data { get; set; } = null!;
    
    [JsonPropertyName("token")]
    public string Token { get; set; }  = null!;
}

public class AuthErrorResponse : IAuthResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }  = null!;
}

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