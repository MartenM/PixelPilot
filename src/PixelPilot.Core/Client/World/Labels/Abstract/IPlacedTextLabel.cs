using System.Drawing;

namespace PixelPilot.Client.World.Labels;

public interface IPlacedTextLabel
{
    public string Id { get; }
    
    public ITextLabel Label { get; }
}