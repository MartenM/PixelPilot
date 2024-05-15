using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

/// <summary>
/// Represents a reflective pixel game packet for outgoing communication.
/// Automatically converts to binary.
/// </summary>
public class ReflectivePixelOutPacket : IPixelGamePacketOut
{

    protected WorldMessageType messageType;

    public ReflectivePixelOutPacket(WorldMessageType messageType)
    {
        this.messageType = messageType;
    }

    /// <summary>
    /// Gets the fields of the packet.
    /// </summary>
    /// <param name="allowNull">Flag to allow null fields.</param>
    /// <returns>A list of fields.</returns>
    protected List<dynamic> GetFields(bool allowNull)
    {
        // Get all new fields IN ORDER but not the 'type' field.
        var properties = GetType().GetProperties();

        var fieldValues = new List<dynamic>();
        foreach (var property in properties)
        {
            // Skip the message type. We write that manually already.
            if (property.PropertyType.Name.Equals("WorldMessageType")) continue;

            var value = property.GetValue(this);
            if (value == null)
            {
                if (!allowNull) throw new InvalidOperationException("Packet fields cannot be NULL!");
                continue;
            }
            
            fieldValues.Add(value);
        }

        return fieldValues;
    }

    /// <summary>
    /// Gets the fields of the packet.
    /// </summary>
    /// <returns>A list of fields.</returns>
    protected virtual List<dynamic> GetFields()
    {
        return GetFields(false);
    }

    /**
     * Converts a given packet to binary
     */
    public byte[] ToBinaryPacket()
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write((byte) MessageType.World);
        writer.Write7BitEncodedInt((int) messageType);

        // This kind of sucks, but it does work.
        // A well you can't have it all can you?
        Dictionary<Type, int> typeDict = new Dictionary<Type, int>()
        {
            {typeof(string), 0},
            {typeof(bool), 1},
            {typeof(byte), 2},
            {typeof(Int16), 3},
            {typeof(Int32), 4},
            {typeof(Int64), 5},
            {typeof(byte[]), 6},
            {typeof(double), 7},
        };

        var fieldValues = GetFields();
        for (int i = 0; i < fieldValues.Count; i++)
        {
            var fieldValue = fieldValues[i];
            var fieldType = fieldValue.GetType();

            byte[] bytes;
            switch (typeDict[fieldType])
            {
                case 0: // string
                    bytes = System.Text.Encoding.UTF8.GetBytes((string) fieldValue);
                    writer.Write7BitEncodedInt((int) PacketFieldType.String);
                    writer.Write7BitEncodedInt(bytes.Length);
                    writer.Write(bytes);
                    break;
                case 1: // bool
                    writer.Write7BitEncodedInt((int) PacketFieldType.Boolean);
                    writer.Write(Convert.ToByte(fieldValue));
                    break;
                case 2: // byte
                    writer.Write7BitEncodedInt((int) PacketFieldType.Byte);
                    writer.Write(Convert.ToByte(fieldValue));
                    break;
                case 3: // int16
                    writer.Write7BitEncodedInt((int) PacketFieldType.Int16);
                    bytes = BitConverter.GetBytes(fieldValue);
                    Array.Reverse(bytes);
                    writer.Write(bytes);
                    break;
                case 4: // int32
                    writer.Write7BitEncodedInt((int) PacketFieldType.Int32);
                    bytes = BitConverter.GetBytes(fieldValue);
                    Array.Reverse(bytes);
                    writer.Write(bytes);
                    break;
                case 5: // int64
                    writer.Write7BitEncodedInt((int) PacketFieldType.Int64);
                    bytes = BitConverter.GetBytes(fieldValue);
                    Array.Reverse(bytes);
                    writer.Write(bytes);
                    break;
                case 7: // double
                    writer.Write7BitEncodedInt((int) PacketFieldType.Double);
                    bytes = BitConverter.GetBytes(fieldValue);
                    Array.Reverse(bytes);
                    writer.Write(bytes);
                    break;
                case 6: // byte[]
                    var input = (byte[])fieldValue;
                    writer.Write7BitEncodedInt((int) PacketFieldType.ByteArray);
                    writer.Write7BitEncodedInt(input.Length);
                    writer.Write(input);
                    break;
                default:
                    throw new Exception($"Could not deserialize type. {fieldType} at {memoryStream.Position}");
            }
        }

        return memoryStream.ToArray();
    }
}