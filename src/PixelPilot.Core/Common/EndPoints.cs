namespace PixelPilot.Common;

public static class EndPoints
{
    public const string SiteUrl = "pw-staging.rnc.priddle.nl";
    
    public const string ApiEndpoint = $"https://api.{SiteUrl}";
    public const string GameHttpEndpoint = $"https://server.{SiteUrl}";
    public const string GameWebsocketEndpoint = $"wss://server.{SiteUrl}";
}