using System.Drawing;
using PixelPilot.Client.World.Blocks.Types;
using PixelPilot.Client.World.Constants;

namespace PixelGameTests;

public class ColorTests
{
    [Test]
    public void TestColorToBuffer()
    {
        var primaryColor = Color.FromArgb(111, 222, 123);
        
        // From ColoredBlock
        uint rawColour  =
            ((uint) primaryColor.A << 24) |
            ((uint) primaryColor.R << 16) |
            ((uint) primaryColor.G << 8) |
            ((uint) primaryColor.B);
        
        // Reconstruct from PixelWorld buffer
        var color = Color.FromArgb(
            (int)((rawColour) >> 24 & 0xFF), // Alpha
            (int)((rawColour >> 16) & 0xFF), // Red
            (int)((rawColour >> 8) & 0xFF), // Green
            (int)(rawColour & 0xFF) // Blue
        );
        
        Assert.That(color, Is.EqualTo(primaryColor));
    }
}