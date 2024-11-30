namespace PixelPilot.Api.Responses.visible;

public class PixelWalkerWorldsResponse
{
    public int OnlineRoomCount { get; set; }
    public int OnlinePlayerCount { get; set; }
    public List<VisibleRoom> VisibleRooms { get; set; } = null!;
}