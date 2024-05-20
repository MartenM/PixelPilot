using CommandLine;
using PixelPilot.DebugTools.Tools;

namespace PixelPilot.DebugTools;

public class DebugTools
{
    public class Options
    {
        [Option('t', "tool", Required = true, HelpText = "The debug tool you want to use.")]
        public string Tool { get; set; }
        
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }

    static void Main(string[] args)
    {
        Console.WriteLine($"Args: {String.Join(", ", args)}");
        Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
        
    }

    public static void Run(Options options)
    {
        Dictionary<string, IDebugTool> tools = new Dictionary<string, IDebugTool>()
        {
            {"packet-out", new OutgoingPacketDecoder()}
        };

        if (!tools.TryGetValue(options.Tool, out var tool))
        {
            Console.WriteLine("The specified tool could not be found.");
            return;
        }
        
        Console.WriteLine($"Starting {options.Tool} tool...");
        tool.Execute();
    }
}