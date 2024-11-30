using System.Reflection;
using Google.Protobuf;
using PixelPilot.Client.Messages.Packets.Exceptions;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Packets.Extensions;

public static class WorldPacketExtensions
{
    private static readonly Type WorldPacketType = typeof(WorldPacket);
    private static Dictionary<string, MethodInfo> _packetMethods = CreateMethodInfoDict();
    
    public static IMessage GetPacket(this WorldPacket packet)
    {
        // Get the descriptor for the message type
        var message = packet as IMessage;

        if (message == null) throw new Exception("Message cannot be null.");
        
        var descriptor = message.Descriptor;

        // Sanity check.
        if (descriptor.Oneofs.Count != 1) throw new Exception("Another oneof field was added, but unexpected!");
        
        var packetField = descriptor.Oneofs.First();
        var populatedField = packetField.Accessor.GetCaseFieldDescriptor(message);
        var innerPacket = populatedField.Accessor.GetValue(packet) as IMessage;

        if (innerPacket == null) throw new Exception("Failed to extract packet from message.");
        return innerPacket;
    }

    /**
     * Allows for easy construction of a packet -> WorldPacket wrapping.
     */
    public static WorldPacket ToWorldPacket(this IMessage message)
    {
        // Create an empty packet
        var packet = new WorldPacket();

        // Get the packet name and the corresponding property in WorldPacket
        var packetName = message.Descriptor.Name;

        if (!_packetMethods.TryGetValue(packetName, out var setter))
        {
            throw new PacketWrapException(packetName, WorldPacket.Descriptor.Name);
        }

        // Invoke the setter.
        setter.Invoke(packet, new object[] { message });

        // Retur the new WorldPacket.
        return packet;
    }

    /// <summary>
    /// Populate the dict so that access is O(1) instead of a loop for each packet all the time.
    /// </summary>
    private static Dictionary<string, MethodInfo> CreateMethodInfoDict()
    {
        var dict = new Dictionary<string, MethodInfo>();
        
        foreach (var prop in WorldPacketType.GetProperties())
        {
            var setMethod = prop.GetSetMethod();
            if (setMethod == null) continue;
            
            dict.Add(prop.Name, setMethod);
        }

        return dict;
    }
}