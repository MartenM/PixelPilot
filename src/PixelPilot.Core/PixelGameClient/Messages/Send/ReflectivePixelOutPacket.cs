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

        var fieldValues = GetFields();
        for (int i = 0; i < fieldValues.Count; i++)
        {
            PacketConverter.WriteTypeBe(writer, fieldValues[i]);
        }

        return memoryStream.ToArray();
    }
}