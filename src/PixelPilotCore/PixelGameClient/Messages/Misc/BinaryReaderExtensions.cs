using System.Buffers.Binary;

namespace PixelPilot.PixelGameClient.Messages.Misc;

/**
 * Helper methods from: https://stackoverflow.com/a/15274591/6152815
 */
public static class BinaryReaderExtensions
{
    public static byte[] Reverse(this byte[] b)
    {
        Array.Reverse(b);
        return b;
    }

    public static UInt16 ReadUInt16BE(this BinaryReader binRdr)
    {
        return BitConverter.ToUInt16(binRdr.ReadBytesRequired(sizeof(UInt16)).Reverse(), 0);
    }

    public static Int16 ReadInt16BE(this BinaryReader binRdr)
    {
        return BitConverter.ToInt16(binRdr.ReadBytesRequired(sizeof(Int16)).Reverse(), 0);
    }

    public static UInt32 ReadUInt32BE(this BinaryReader binRdr)
    {
        return BitConverter.ToUInt32(binRdr.ReadBytesRequired(sizeof(UInt32)).Reverse(), 0);
    }

    public static Int32 ReadInt32BE(this BinaryReader binRdr)
    {
        return BitConverter.ToInt32(binRdr.ReadBytesRequired(sizeof(Int32)).Reverse(), 0);
    }

    public static double ReadDoubleBE(this BinaryReader binaryReader)
    {
        return BinaryPrimitives.ReadDoubleBigEndian(new ReadOnlySpan<byte>(binaryReader.ReadBytes(8)));
    }

    public static byte[] ReadBytesRequired(this BinaryReader binRdr, int byteCount)
    {
        var result = binRdr.ReadBytes(byteCount);

        if (result.Length != byteCount)
            throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

        return result;
    }
}