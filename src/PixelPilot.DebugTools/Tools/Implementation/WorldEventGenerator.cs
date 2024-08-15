using PixelPilot.PixelHttpClient;

namespace PixelPilot.DebugTools.Tools.Implementation;

public class WorldEventGenerator : IDebugTool
{
    public void Execute()
    {
        var api = new PixelApiClient("Unused");
        var task = Task.Run(async () =>
        {
            var response = await api.GetMessageTypes();

            Console.WriteLine("Enum entries:\n\n");
            foreach (var line in response)
            {
                Console.WriteLine($"{line},");
            }
        });

        task.Wait();
    }
}