using Google.Protobuf;

namespace PixelPilot.Client.World.Zones;

/// <summary>
/// Encodes/decodes a zone's per-block membership mask.
/// Wire format (confirmed against PixelWalker.Game/Models/Zone.cs): cells are walked column-major
/// (x outer, y inner; index = x * height + y). The first byte is the raw starting boolean (0x00/0x01,
/// not a varint), followed by alternating varint-encoded run lengths (protobuf/.NET 7-bit encoded
/// ints) starting from that value. An empty grid (width * height == 0) serializes to zero bytes.
/// </summary>
public static class MembershipRle
{
    public static bool[,] Decode(ByteString rle, int width, int height)
    {
        var grid = new bool[width, height];
        var totalCells = width * height;
        if (totalCells == 0) return grid;

        var bytes = rle.Span;
        if (bytes.Length == 0) return grid;

        var current = bytes[0] != 0;
        var index = 0;
        var offset = 1;

        while (index < totalCells && offset < bytes.Length)
        {
            var runLength = ReadVarint(bytes, ref offset);
            for (var i = 0; i < runLength && index < totalCells; i++, index++)
            {
                var x = index / height;
                var y = index % height;
                grid[x, y] = current;
            }

            current = !current;
        }

        return grid;
    }

    public static ByteString Encode(bool[,] grid)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        var totalCells = width * height;
        if (totalCells == 0) return ByteString.Empty;

        bool GetByIndex(int idx)
        {
            var x = idx / height;
            var y = idx % height;
            return grid[x, y];
        }

        using var stream = new MemoryStream();
        stream.WriteByte((byte) (GetByIndex(0) ? 1 : 0));

        var index = 0;
        while (index < totalCells)
        {
            var current = GetByIndex(index);
            var runLength = 1;
            while (index + runLength < totalCells && GetByIndex(index + runLength) == current)
            {
                runLength++;
            }

            WriteVarint(stream, runLength);
            index += runLength;
        }

        return ByteString.CopyFrom(stream.ToArray());
    }

    private static int ReadVarint(ReadOnlySpan<byte> bytes, ref int offset)
    {
        var result = 0;
        var shift = 0;

        while (offset < bytes.Length)
        {
            var b = bytes[offset++];
            result |= (b & 0x7F) << shift;

            if ((b & 0x80) == 0)
            {
                break;
            }

            shift += 7;
        }

        return result;
    }

    private static void WriteVarint(Stream stream, int value)
    {
        var v = (uint) value;
        do
        {
            var b = (byte) (v & 0x7F);
            v >>= 7;
            if (v != 0)
            {
                b |= 0x80;
            }

            stream.WriteByte(b);
        } while (v != 0);
    }
}
