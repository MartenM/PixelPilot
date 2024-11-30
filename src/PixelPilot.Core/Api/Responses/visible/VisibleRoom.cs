using System.Text.Json.Serialization;

namespace PixelPilot.Api.Responses.visible;

public class VisibleRoom
{
    public string Id { get; set; } = null!;
    public int Players { get; set; }
    
    [JsonPropertyName("max_players")]
    public int MaxPlayers { get; set; }

    public VisibleRoomData Data { get; set; } = null!;

    public string Title => Data.Title;
    public int Plays => Data.Plays;
}