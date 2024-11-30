using System.Drawing;
using Google.Protobuf;
using Google.Protobuf.Collections;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Packets.Extensions;

public static class WorldBlockPacketBuilder
{
    public static WorldBlockPlacedPacket CreatePacket(int x, int y, int layer, int blockId)
    {
        var packet = new WorldBlockPlacedPacket()
        {
            Layer = layer,
            BlockId = blockId,
            IsFillOperation = false,
            ExtraFields = ByteString.Empty
        };
        
        packet.Positions.Add(new PointInteger() { X = x, Y = y });
        
        return packet;
    }
    
    public static WorldBlockPlacedPacket CreatePacket(int x, int y, int layer, int blockId, dynamic[] extraFields)
    {
        var encodedExtra = new BinaryDataList(extraFields).ToByteArray();
        
        var packet = new WorldBlockPlacedPacket()
        {
            Layer = layer,
            BlockId = blockId,
            IsFillOperation = false,
            ExtraFields = ByteString.CopyFrom(encodedExtra)
        };
        
        packet.Positions.Add(new PointInteger() { X = x, Y = y });
        
        return packet;
    }
    
    public static WorldBlockPlacedPacket CreatePacket(List<Point> positions, int layer, int blockId)
    {
        var packet = new WorldBlockPlacedPacket()
        {
            Layer = layer,
            BlockId = blockId,
            IsFillOperation = false,
            ExtraFields = ByteString.Empty
        };

        foreach (var position in positions)
        {
            packet.Positions.Add(new PointInteger() { X = position.X, Y = position.Y });
        }
        
        return packet;
    }
    
    public static WorldBlockPlacedPacket CreatePacket(List<Point> positions, int layer, int blockId, dynamic[] extraFields)
    {
        var encodedExtra = new BinaryDataList(extraFields).ToByteArray();
        
        var packet = new WorldBlockPlacedPacket()
        {
            Layer = layer,
            BlockId = blockId,
            IsFillOperation = false,
            ExtraFields = ByteString.CopyFrom(encodedExtra)
        };
        
        foreach (var position in positions)
        {
            packet.Positions.Add(new PointInteger() { X = position.X, Y = position.Y });
        }
        
        return packet;
    }
}