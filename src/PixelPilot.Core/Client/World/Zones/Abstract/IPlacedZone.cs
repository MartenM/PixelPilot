namespace PixelPilot.Client.World.Zones;

public interface IPlacedZone
{
    public string Id { get; }

    public IZone Zone { get; }
}
