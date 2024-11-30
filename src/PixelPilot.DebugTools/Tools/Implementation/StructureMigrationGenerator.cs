using System.Text.Json;
using PixelPilot.Api.Responses;

namespace PixelPilot.DebugTools.Tools.Implementation;

public class StructureMigrationGenerator : CommandLineTool
{
    public override string BeforeEach { get; } = "Enter the full path: ";

    protected override void ExecuteCommand(string[] args, string fullText)
    {
        string jsonString = File.ReadAllText(fullText);
        
        Dictionary<string, string>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

        if (keyValuePairs == null)
        {
            Console.WriteLine("Something went wrong while loading the JSON.");
            return;
        }
        
        foreach (var kvp in keyValuePairs)
        {
            Console.WriteLine($"\"{MappingsResponse.ToCName(kvp.Key)}\" => \"{MappingsResponse.ToCName(kvp.Value)}\",");
        }
    }
}