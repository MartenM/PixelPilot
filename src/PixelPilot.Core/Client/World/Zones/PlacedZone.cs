namespace PixelPilot.Client.World.Zones;

public class PlacedZone : Zone, IPlacedZone
{
    public string Id { get; set; } = null!;
    public IZone Zone { get; set; } = null!;
}
