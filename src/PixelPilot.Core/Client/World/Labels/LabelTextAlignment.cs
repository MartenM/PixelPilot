using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Labels;

public enum LabelTextAlignment
{
    Left,
    Center,
    Right
}

public static class LabelTextAlignmentExtensions
{
    public static LabelTextAlignment ToLabelTextAlignment(this TextAlignment alignment)
    {
        switch (alignment)
        {
            case TextAlignment.Left:
                return LabelTextAlignment.Left;
                break;
            case TextAlignment.Center:
                return LabelTextAlignment.Center;
                break;
            case TextAlignment.Right:
                return LabelTextAlignment.Right;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
        }
    }
    
    public static TextAlignment ToProtoTextLabelAlignment(this LabelTextAlignment alignment)
    {
        switch (alignment)
        {
            case LabelTextAlignment.Left:
                return TextAlignment.Left;
                break;
            case LabelTextAlignment.Center:
                return TextAlignment.Center;
                break;
            case LabelTextAlignment.Right:
                return TextAlignment.Right;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
        }
    }
}