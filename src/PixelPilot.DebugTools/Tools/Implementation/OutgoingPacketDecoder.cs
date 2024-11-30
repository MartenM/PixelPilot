using System.Text;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.DebugTools.Tools.Implementation;

/// <summary>
/// Decode outgoing packets.
/// </summary>
public class OutgoingPacketDecoder : CommandLineTool
{
    public override string BeforeEach { get; } = "Packet HEX: ";

    protected override void ExecuteCommand(string[] args, string fullText)
    {
        
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