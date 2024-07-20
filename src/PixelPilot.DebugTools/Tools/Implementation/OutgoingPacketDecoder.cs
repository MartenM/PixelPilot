using System.Text;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.DebugTools.Tools.Implementation;

/// <summary>
/// Decode outgoing packets.
/// </summary>
public class OutgoingPacketDecoder : CommandLineTool
{
    public override string BeforeEach { get; } = "Packet HEX: ";

    protected override void ExecuteCommand(string[] args, string fullText)
    {
        // Parse the incoming to a binary[]
        var hexInput = string.Join("", args);
        byte[] data = new byte[hexInput.Length / 2];
        for (int i = 0; i < hexInput.Length; i += 2)
        {
            data[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
        }
        
        Console.WriteLine($"Input: \t\t{FormatHex(hexInput)}");
        Console.WriteLine($"Output: \t{BitConverter.ToString(data)}");

        var converter = new PacketConverter();
        var stream = new MemoryStream(data);
        var reader = new BinaryReader(stream);

        var mType = (MessageType) reader.ReadByte();
        if (mType != MessageType.World)
        {
            Console.WriteLine("Not a world message type.");
            return;
        }
        
        var worldMessageType = (WorldMessageType) reader.Read7BitEncodedInt();
        
        // Get all fields.
        var fields = new List<dynamic>();
        while (stream.Position < stream.Length)
        {
            var fieldType = (PacketFieldType) reader.ReadByte();
            fields.Add(PacketConverter.ReadType(reader, fieldType));
        }
        
        
        Console.WriteLine($"Packet type: \t{worldMessageType}");
        Console.WriteLine($"Types: \t\t{String.Join(", ", fields.Select(f => f.GetType().ToString()))}");
        Console.WriteLine($"Fields: \t{String.Join(", ", fields.Select(f => f.ToString()))}");
        Console.WriteLine($"Combined: \t{string.Join(", ", fields.Select(f => f + " " + ((Type)f.GetType()).Name))}");
        Console.WriteLine(" ");
    }

    private static string FormatHex(string hexInput)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < hexInput.Length; i++)
        {
            if (i > 0 && i % 2 == 0)
            {
                builder.Append('-');
            }

            builder.Append(hexInput[i]);
        }

        return builder.ToString().ToUpper();
    }
}