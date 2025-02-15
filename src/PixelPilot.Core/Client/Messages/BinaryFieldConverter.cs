using System.Text;
using Microsoft.Extensions.Logging;
using PixelPilot.Client.Messages.Constants;
using PixelPilot.Client.Messages.Exceptions;
using PixelPilot.Client.Messages.Misc;
using PixelPilot.Common.Logging;

namespace PixelPilot.Client.Messages;

/// <summary>
/// Converts binary data into pixel game packets.
/// </summary>
public class BinaryFieldConverter
{
    private static readonly ILogger _logger = LogManager.GetLogger("PacketConverter");
    
    /// <summary>
    /// Read from a binary stream and output the correct objects.
    /// </summary>
    /// <param name="binary">The binary data representing the packet.</param>
    /// <returns>The constructed array</returns>
    public static dynamic[] ConstructBinaryDataList(byte[] binary)
    {
        using var memoryStream = new MemoryStream(binary);
        using var reader = new BinaryReader(memoryStream);
            
        // Now parse all fields.
        var fields = new List<dynamic>();
        while (memoryStream.Position < memoryStream.Length)
        {
            var fieldType = (PacketFieldType) reader.ReadByte();
            fields.Add(ReadType(reader, fieldType));
        }

        return fields.ToArray();
    }

    /// <summary>
    /// Reads data of a specified type from a BinaryReader according to the provided PacketFieldType.
    /// </summary>
    /// <param name="reader">The BinaryReader to read from.</param>
    /// <param name="fieldType">The type of data to read.</param>
    /// <returns>The data read from the BinaryReader.</returns>
    /// <exception cref="Exception">Thrown when the provided fieldType is not supported.</exception>
    public static dynamic ReadType(BinaryReader reader, PacketFieldType fieldType)
    {
        switch (fieldType)
        {
            case PacketFieldType.String:
                return reader.ReadString();
            case PacketFieldType.Boolean:
                return reader.ReadBoolean();
            case PacketFieldType.Byte:
                return reader.ReadByte();
            case PacketFieldType.Int16:
                return reader.ReadInt16BE();
            case PacketFieldType.Int32:
                return reader.ReadInt32BE();
            case PacketFieldType.Int64:
                return reader.ReadInt64();
            case PacketFieldType.ByteArray:
                var length = reader.Read7BitEncodedInt();
                return reader.ReadBytes(length);
            case PacketFieldType.Double:
                return reader.ReadDoubleBE();
            case PacketFieldType.UInt32:
                return reader.ReadUInt32();
            default:
                throw new Exception($"Could not deserialize type. {fieldType}");
        }
    }
    
    /// <summary>
    /// Reads data of a specified type from a BinaryReader in little-endian format according to the provided PacketFieldType.
    /// </summary>
    /// <param name="reader">The BinaryReader to read from.</param>
    /// <param name="fieldType">The type of data to read.</param>
    /// <returns>The data read from the BinaryReader.</returns>
    /// <exception cref="Exception">Thrown when the provided fieldType is not supported.</exception>
    public static dynamic ReadTypeLe(BinaryReader reader, PacketFieldType fieldType)
    {
        switch (fieldType)
        {
            case PacketFieldType.String:
                return reader.ReadString();
            case PacketFieldType.Boolean:
                return reader.ReadBoolean();
            case PacketFieldType.Byte:
                return reader.ReadByte();
            case PacketFieldType.Int16:
                return reader.ReadInt16();
            case PacketFieldType.Int32:
                return reader.ReadInt32();
            case PacketFieldType.Int64:
                return reader.ReadInt64();
            case PacketFieldType.ByteArray:
                var length = reader.Read7BitEncodedInt();
                return reader.ReadBytes(length);
            case PacketFieldType.Double:
                return reader.ReadDoubleBE();
            case PacketFieldType.UInt32:
                return reader.ReadUInt32();
            default:
                throw new Exception($"Could not deserialize type. {fieldType}");
        }
    }

    /// <summary>
    /// Logs an error when there is an issue with converting packet types.
    /// Includes the received fields and expected constructors.
    /// </summary>
    /// <param name="receivedFields">The list of fields received.</param>
    /// <param name="packetType">The type of the packet.</param>
    /// <param name="dynamicConstructor">If the constructor needed to be dynamic</param>
    private static void LogPacketTypeConversionError(IReadOnlyCollection<dynamic> receivedFields, Type packetType, bool dynamicConstructor)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"The packet type ({packetType.Name}) was found, but no {(dynamicConstructor ? "dynamic" : "")} constructor matching the message could be found.");
        builder.AppendLine($"Received packet: \t {String.Join(", ", receivedFields.Select(f => f.GetType().ToString()))}");
        foreach (var constructor in packetType.GetConstructors())
        {
            builder.AppendLine($"Constructor: \t {String.Join(", ", constructor.GetParameters().Select(info => info.ParameterType))}");
        }
        
        _logger.LogWarning(builder.ToString());
        _logger.LogWarning($"Fields: {String.Join(", ", receivedFields.Select(f => f.ToString()))}");
    }

    /// <summary>
    /// Returns a list of field types. Used for debugging new messages.
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    private static string FieldTypes(List<dynamic> fields)
    {
        return string.Join(", ", fields.Select(f => f + " " + ((Type)f.GetType()).Name));
    }

    private static Dictionary<Type, int> _typeDict = new()
    {
        {typeof(string), 0},
        {typeof(bool), 1},
        {typeof(byte), 2},
        {typeof(short), 3},
        {typeof(int), 4},
        {typeof(long), 5},
        {typeof(byte[]), 6},
        {typeof(double), 7},
        {typeof(uint), 8},
    };

    public static void WriteTypeBe(BinaryWriter writer, dynamic fieldValue)
    {
        var fieldType = fieldValue.GetType();

        byte[] bytes;
        switch (_typeDict[fieldType])
        {
            case 0: // string
                bytes = Encoding.UTF8.GetBytes((string)fieldValue);
                writer.Write7BitEncodedInt((int)PacketFieldType.String);
                writer.Write7BitEncodedInt(bytes.Length);
                writer.Write(bytes);
                break;
            case 1: // bool
                writer.Write7BitEncodedInt((int)PacketFieldType.Boolean);
                writer.Write(Convert.ToByte(fieldValue));
                break;
            case 2: // byte
                writer.Write7BitEncodedInt((int)PacketFieldType.Byte);
                writer.Write(Convert.ToByte(fieldValue));
                break;
            case 3: // int16
                writer.Write7BitEncodedInt((int)PacketFieldType.Int16);
                bytes = BitConverter.GetBytes(fieldValue);
                Array.Reverse(bytes);
                writer.Write(bytes);
                break;
            case 4: // int32
                writer.Write7BitEncodedInt((int)PacketFieldType.Int32);
                bytes = BitConverter.GetBytes(fieldValue);
                Array.Reverse(bytes);
                writer.Write(bytes);
                break;
            case 5: // int64
                writer.Write7BitEncodedInt((int)PacketFieldType.Int64);
                bytes = BitConverter.GetBytes(fieldValue);
                Array.Reverse(bytes);
                writer.Write(bytes);
                break;
            case 7: // double
                writer.Write7BitEncodedInt((int)PacketFieldType.Double);
                bytes = BitConverter.GetBytes(fieldValue);
                Array.Reverse(bytes);
                writer.Write(bytes);
                break;
            case 6: // byte[]
                var input = (byte[])fieldValue;
                writer.Write7BitEncodedInt((int)PacketFieldType.ByteArray);
                writer.Write7BitEncodedInt(input.Length);
                writer.Write(input);
                break;
            case 8: // Uint32
                writer.Write7BitEncodedInt((int)PacketFieldType.UInt32);
                bytes = BitConverter.GetBytes(fieldValue);
                Array.Reverse(bytes);
                writer.Write(bytes);
                break;
            default:
                throw new Exception($"Could not deserialize type: {fieldType}");
        }
    }
}