using PixelPilot.Api;

namespace PixelPilot.DebugTools.Tools.Implementation;

public class MappingGenerator : IDebugTool
{
    public void Execute()
    {
        var api = new PixelApiClient("Unused");
        var task = Task.Run(async () =>
        {
            var response = await api.GetMappings();
            if (response == null) return;

            Console.WriteLine("Enum entries:\n\n");
            foreach (var line in response.AsEnumEntries())
            {
                Console.WriteLine($"{line},");
            }
        });

        task.Wait();
    }
}