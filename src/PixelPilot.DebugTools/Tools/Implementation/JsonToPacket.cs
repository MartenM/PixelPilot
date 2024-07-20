using System.Text.Json;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.DebugTools.Tools.Implementation;

public class JsonToPacket : CommandLineTool
{
    
    protected override void ExecuteCommand(string[] args, string fullText)
    {
        var fullString = String.Join(" ", args);

        try
        {
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true
            };
            
            var packet = JsonSerializer.Deserialize(fullString, typeof(PlayerLocalSwitchResetOutPacket));

            Console.WriteLine(JsonSerializer.Serialize(packet));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}