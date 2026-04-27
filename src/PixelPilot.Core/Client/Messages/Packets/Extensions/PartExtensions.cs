using System.Drawing;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Packets.Extensions;

public static class PartExtensions
{
    public static Point ToPoint(this PointInteger point) => new Point(point.X, point.Y);
    
    public static PointInteger ToPoint(this Point point) => new PointInteger()
    {
        X = point.X,
        Y = point.Y
    };
}