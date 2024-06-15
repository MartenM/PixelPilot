using System.Text;
using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient.Messages.Constants;
using PixelPilot.PixelGameClient.Messages.Exceptions;
using PixelPilot.PixelGameClient.Messages.Misc;
using PixelPilot.PixelGameClient.Messages.Received;

namespace PixelPilot.PixelGameClient.Messages;

/// <summary>
/// Converts binary data into pixel game packets.
/// </summary>
public class PacketConverter
{
    private static readonly ILogger _logger = LogManager.GetLogger("PacketConverter");
    
    /// <summary>
    /// Constructs a pixel game packet from the given binary data.
    /// </summary>
    /// <param name="binary">The binary data representing the packet.</param>
    /// <returns>The constructed pixel game packet.</returns>
    public static IPixelGamePacket ConstructPacket(byte[] binary)
    {
        using var memoryStream = new MemoryStream(binary);
        using var reader = new BinaryReader(memoryStream);
        var type = (MessageType) reader.ReadByte();
        if (type == MessageType.Ping) return new PingPacket();

        var worldMessageType = (WorldMessageType) reader.Read7BitEncodedInt();
            
        // Now parse all fields.
        var fields = new List<dynamic>();
        while (memoryStream.Position < memoryStream.Length)
        {
            var fieldType = (PacketFieldType) reader.ReadByte();
            fields.Add(ReadType(reader, fieldType));
        }
            
        // Get the correct type
        var packetType = worldMessageType.AsPacketType();
        if (packetType == null)
        {
            _logger.LogError($"Failed to construct the ({worldMessageType}) packet. Are you sure the API is up-to-date? ({FieldTypes(fields)})");
            throw new PacketTypeNotFoundException(worldMessageType);
        }

        // Find constructor based on length and type.
        var dynamicConstructor = typeof(IDynamicConstructedPacket).IsAssignableFrom(packetType);
        var constructorInfo = packetType.GetConstructors().FirstOrDefault(con =>
        {
            // If a dynamic constructor should be present. Accept that instead.
            if (dynamicConstructor)
            {
                // Just check length, don't check type. We hopefully don't house idiots that violate the contract.
                if (con.GetParameters().Length != 1) return false;
                return true;
            }
            
            if (con.GetParameters().Length != fields.Count) return false;
            if (con.GetParameters().Where((info, i) => info.ParameterType != fields[i].GetType()).Any()) return false;
            return con.GetParameters().Length == fields.Count;
        });
        
        if (constructorInfo == null)
        {
            LogPacketTypeConversionError(fields, packetType, dynamicConstructor);
            throw new PacketConstructorException(fields, packetType);
        }
            
        // Construct the packet and return it.
        IPixelGamePacket packet = (IPixelGamePacket) (dynamicConstructor ? constructorInfo.Invoke([fields]) : constructorInfo.Invoke(fields.ToArray()));
        return packet;
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
            default:
                throw new Exception($"Could not deserialize type: {fieldType}");
        }
    }
}