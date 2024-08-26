using PixelPilot.PixelHttpClient;

namespace PixelPilot.DebugTools.Tools.Implementation;

public class PixelBlockUpdater : IDebugTool
{
    public void Execute()
    {
        var fileLocation = Environment.GetEnvironmentVariable("PIXELBLOCK_PATH");
        if (fileLocation == null)
        {
            throw new Exception("ENV variable PIXELBLOCK_PATH was not set properly during execution.");
        }

        var lines = File.ReadAllLines(fileLocation).ToList();
        
        // Remove all current lines
        int startIndex = lines.FindIndex(s => s.Contains("{")) + 1;
        int endIndex = lines.FindIndex(s => s.Contains("}")) - 1;
        
        // Calculate how many items to remove
        int count = endIndex - startIndex + 1;
        
        lines.RemoveRange(startIndex, count);
        
        // Now add the new entries
        var api = new PixelApiClient("Unused");
        var response = api.GetMappings();
        response.Wait();
        
        if (response.Result == null) throw new Exception("Could not fetch the mappings!");

        int index = startIndex;
        foreach (var line in response.Result.AsEnumEntries())
        {
            lines.Insert(index, $"    {line},");
            index++;
        }
        
        File.WriteAllLines(fileLocation, lines);
    }
}