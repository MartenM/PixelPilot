using System.Drawing;
using PixelPilot.Api;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Constants;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Blocks.V2;

/// <summary>
/// The FlexBlock is new implementation of block data. It has a base, the block ID, and additional extra fields.
/// </summary>
public class FlexBlock : IPixelBlock
{
    public Dictionary<string, object> Fields;

    public FlexBlock(int blockId)
    {
        BlockId = blockId;
        Fields = new Dictionary<string, object>();
    }
    
    public FlexBlock(PixelBlock block)
    {
        BlockId = (int) block;
        Fields = new Dictionary<string, object>();
    }

    public FlexBlock(int blockId, Dictionary<string, object> fields)
    {
        BlockId = blockId;
        Fields = fields;
    }
    
    public FlexBlock(PixelBlock block, Dictionary<string, object> fields)
    {
        BlockId = (int) block;
        Fields = fields;
    }

    public object Clone()
    {
        return new FlexBlock(BlockId, new Dictionary<string, object>(Fields));
    }

    public int BlockId { get; }
    public PixelBlock Block => (PixelBlock) BlockId;

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
            BlockFieldValue.ValueOneofCase.Int32Value => proto.Int32Value,
            BlockFieldValue.ValueOneofCase.Uint32Value => proto.Uint32Value,
            BlockFieldValue.ValueOneofCase.StringValue => proto.StringValue,
            BlockFieldValue.ValueOneofCase.BoolValue => proto.BoolValue,
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
        return new PlacedBlock(x, y, layer, this);
    }

    public byte[] AsWorldBuffer(int x, int y, int layer)
    {
        throw new PixelApiException("This method is no longer supported.");
    }

    public byte[] AsWorldBuffer(int x, int y, int layer, int customId)
    {
        throw new PixelApiException("This method is no longer supported.");
    }

    protected bool Equals(FlexBlock other)
    {
        return BlockId == other.BlockId && EqualFields(other);
    }

    private bool EqualFields(FlexBlock other)
    {
        return Fields.Count == other.Fields.Count
               && Fields.All(kvp => other.Fields.TryGetValue(kvp.Key, out var value) && EqualFields(kvp.Value, value));
    }

    private bool EqualFields(object a, object b)
    {
        if (a is byte[] ab && b is byte[] bb)
        {
            return ab.SequenceEqual(bb);
        }

        return a.Equals(b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FlexBlock)obj);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(BlockId);

        foreach (var kvp in Fields.OrderBy(k => k.Key))
        {
            hash.Add(kvp.Key);
            hash.Add(kvp.Value);
        }

        return hash.ToHashCode();
    }
}