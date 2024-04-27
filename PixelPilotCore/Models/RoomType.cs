namespace PixelPilot.Models;

public enum RoomType
{
    Pixelwalker3
}

public static class RoomTypeExtensions
{
    public static string AsString(this RoomType type)
    {
        switch (type)
        {
            case RoomType.Pixelwalker3:
                return "pixelwalker3";
            default:
                throw new NotImplementedException("This room type does not exist (yet!)");
        }
    }
}