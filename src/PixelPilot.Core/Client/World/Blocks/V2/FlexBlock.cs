using System.Drawing;
using PixelPilot.Api;
using PixelPilot.Client.World.Blocks.Placed;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.V2;

public class FlexBlock : IPixelBlock
{
    public Dictionary<string, object> Fields;

    public FlexBlock(int blockId)
    {
        BlockId = blockId;
        Fields = new  Dictionary<string, object>();
    }
    
    public FlexBlock(int blockId, Dictionary<string, object> fields)
    {
        BlockId = blockId;
        Fields = fields;
    }

    public object Clone()
    {
        return new FlexBlock(BlockId, new Dictionary<string, object>(Fields));
    }

    public int BlockId { get; }
    public WorldBlockPlacedPacket AsPacketOut(int x, int y, int layer)
    {
        var packet = new WorldBlockPlacedPacket();
        
        packet.BlockId = BlockId;
        packet.Layer = layer;
        packet.Positions.Add(new PointInteger()
        {
            X = x,
            Y = y,
        });

        foreach (var field in Fields)
        {
            packet.Fields.Add(field.Key, FromObject(field.Value));
        }
        
        return packet;
    }
    
    public static BlockFieldValue FromObject(object value)
    {
        var proto = new BlockFieldValue();

        switch (value)
        {
            case int i:
                proto.Int32Value = i;
                break;

            case uint u:
                proto.Uint32Value = u;
                break;

            case string s:
                proto.StringValue = s;
                break;

            case bool b:
                proto.BoolValue = b;
                break;

            case byte[] bytes:
                proto.ByteArrayValue = Google.Protobuf.ByteString.CopyFrom(bytes);
                break;

            default:
                throw new ArgumentException(
                    $"Unsupported value type: {value?.GetType().FullName}"
                );
        }

        return proto;
    }
    
    public static object ToObject(BlockFieldValue proto)
    {
        return proto.ValueCase switch
        {
            BlockFieldValue.ValueOneofCase.Int32Value     => proto.Int32Value,
            BlockFieldValue.ValueOneofCase.Uint32Value    => proto.Uint32Value,
            BlockFieldValue.ValueOneofCase.StringValue    => proto.StringValue,
            BlockFieldValue.ValueOneofCase.BoolValue      => proto.BoolValue,
            BlockFieldValue.ValueOneofCase.ByteArrayValue => proto.ByteArrayValue.ToByteArray(),
            _ => throw new PixelApiException("Could not convert the BlockFieldValue. Is the API still up to date?")
        };
    }

    public WorldBlockPlacedPacket AsPacketOut(List<Point> positions, int layer)
    {
        var packet = new WorldBlockPlacedPacket();
        
        packet.BlockId = BlockId;
        packet.Layer = layer;
        foreach (var position in positions)
        {
            packet.Positions.Add(new PointInteger()
            {
                X = position.X,
                Y = position.Y,
            });
        }

        foreach (var field in Fields)
        {
            packet.Fields.Add(field.Key, FromObject(field.Value));
        }
        
        return packet;
    }

    public IPlacedBlock AsPlacedBlock(int x, int y, int layer)
    {
        return new PlacedBlock(x, y , layer, this);
    }

    public byte[] AsWorldBuffer(int x, int y, int layer)
    {
        throw new PixelApiException("This method is no longer supported.");
    }

    public byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        throw new PixelApiException("This method is no longer supported.");
    }
}