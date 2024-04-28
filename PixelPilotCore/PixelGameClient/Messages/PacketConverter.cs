using System.Text;
using Microsoft.Extensions.Logging;
using PixelPilot.Models;
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
    private readonly ILogger _logger = LogManager.GetLogger("PacketConverter");
    
    /// <summary>
    /// Constructs a pixel game packet from the given binary data.
    /// </summary>
    /// <param name="binary">The binary data representing the packet.</param>
    /// <returns>The constructed pixel game packet.</returns>
    public IPixelGamePacket ConstructPacket(byte[] binary)
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

            switch (fieldType)
            {
                case PacketFieldType.String:
                    fields.Add(reader.ReadString());
                    break;
                case PacketFieldType.Boolean:
                    fields.Add(reader.ReadBoolean());
                    break;
                case PacketFieldType.Byte:
                    fields.Add(reader.ReadByte());
                    break;
                case PacketFieldType.Int16:
                    fields.Add(reader.ReadInt16BE());
                    break;
                case PacketFieldType.Int32:
                    fields.Add(reader.ReadInt32BE());
                    break;
                case PacketFieldType.Int64:
                    fields.Add(reader.ReadInt64());
                    break;
                case PacketFieldType.ByteArray:
                    var length = reader.Read7BitEncodedInt();
                    fields.Add(reader.ReadBytes(length));
                    break;
                case PacketFieldType.Double:
                    fields.Add(reader.ReadDoubleBE());
                    break;
                default:
                    throw new Exception($"Could not deserialize type. {fieldType} at {memoryStream.Position}");
            }
        }
            
        // Get the correct type
        var packetType = worldMessageType.AsPacketType();
        if (packetType == null)
        {
            _logger.LogError($"Failed to construct the ({worldMessageType}) packet. Are you sure the API is up-to-date? ({FieldTypes(fields)})");
            throw new PacketTypeNotFoundException(worldMessageType);
        }

        // Find constructor based on length and type.
        var constructorInfo = packetType.GetConstructors().FirstOrDefault(con =>
        {
            if (con.GetParameters().Length != fields.Count) return false;
            if (con.GetParameters().Where((info, i) => info.ParameterType != fields[i].GetType()).Any()) return false;
            return con.GetParameters().Length == fields.Count;
        });
        
        if (constructorInfo == null)
        {
            LogPacketTypeConversionError(fields, packetType);
            throw new PacketConstructorException(fields, packetType);
        }
            
        // Construct the packet and return it.
        IPixelGamePacket packet = (IPixelGamePacket) constructorInfo.Invoke(fields.ToArray());
        return packet;
    }

    /// <summary>
    /// Logs an error when there is an issue with converting packet types.
    /// Includes the received fields and expected constructors.
    /// </summary>
    /// <param name="receivedFields">The list of fields received.</param>
    /// <param name="packetType">The type of the packet.</param>
    private void LogPacketTypeConversionError(IReadOnlyCollection<dynamic> receivedFields, Type packetType)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"The packet type ({packetType.Name}) was found, but no constructor matching the message could be found.");
        _logger.LogInformation($"Fields: {String.Join(", ", receivedFields.Select(f => f.ToString()))}");
        builder.AppendLine($"Received packet: \t {String.Join(", ", receivedFields.Select(f => f.GetType().ToString()))}");
        foreach (var constructor in packetType.GetConstructors())
        {
            builder.AppendLine($"Constructor: \t {String.Join(", ", constructor.GetParameters().Select(info => info.ParameterType))}");
        }
        
        _logger.LogWarning(builder.ToString());
    }

    /// <summary>
    /// Returns a list of field types. Used for debugging new messages.
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string FieldTypes(List<dynamic> fields)
    {
        return string.Join(", ", fields.Select(f => f + " " + ((Type)f.GetType()).Name));
    }
}