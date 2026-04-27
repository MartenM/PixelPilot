using System.Drawing;

namespace PixelPilot.Client.World.Labels;

public class PlacedTextLabel : TextLabel, IPlacedTextLabel
{
    public string Id { get; set; } = null!;
    public ITextLabel Label { get; set; } = null!;
}