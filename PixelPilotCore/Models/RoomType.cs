namespace PixelPilot.Models;

public enum RoomType
{
    Pixelwalker4
}

public static class RoomTypeExtensions
{
    public static string AsString(this RoomType type)
    {
        switch (type)
        {
            case RoomType.Pixelwalker4:
                return "pixelwalker4";
            default:
                throw new NotImplementedException("This room type does not exist (yet!)");
        }
    }
}