using System.Drawing;

namespace PixelPilot.Client.Extensions;

public static class ColorExtensions
{
    public static uint ToUint(this Color color)
    {
        uint rawColor = 0;
        rawColor |= ((uint) color.A << 24);
        rawColor |= ((uint) color.R << 16);
        rawColor |= ((uint) color.G << 8);
        rawColor |= ((uint) color.B);

        return rawColor;
    }

    public static Color ToColor(this uint color)
    {
        byte a = (byte)((color >> 24) & 0xFF);
        byte r = (byte)((color >> 16) & 0xFF);
        byte g = (byte)((color >> 8) & 0xFF);
        byte b = (byte)(color & 0xFF);

        return Color.FromArgb(a, r, g, b);
    }
}