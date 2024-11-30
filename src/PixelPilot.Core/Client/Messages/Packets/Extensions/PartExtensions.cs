using System.Drawing;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Packets.Extensions;

public static class PartExtensions
{
    public static Point ToPoint(this PointInteger point) => new Point(point.X, point.Y);
}