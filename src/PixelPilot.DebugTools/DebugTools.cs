using CommandLine;
using PixelPilot.DebugTools.Tools;
using PixelPilot.DebugTools.Tools.Implementation;

namespace PixelPilot.DebugTools;

public class DebugTools
{
    public class Options
    {
        [Option('t', "tool", HelpText = "The debug tool you want to use.")]
        public string? Tool { get; set; } = null!;
        
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
            {"packet-out", new OutgoingPacketDecoder()},
            {"mappings", new MappingGenerator()},
            {"json-packet-out", new JsonToPacket()},
            {"struct-migration", new StructureMigrationGenerator()},
            {"event-types", new WorldEventGenerator()},
            {"update-pixelblock", new PixelBlockUpdater()},
            {"update-packets", new WorldPacketTypesUpdater()}
        };

        if (options.Tool == null)
        {
            Console.WriteLine($"Available tools: {String.Join(", ", tools.Select(t => t.Key))}");
            Console.Write("Please pick a tool: ");
            options.Tool = Console.ReadLine();
        }
        
        if (options.Tool == null || !tools.TryGetValue(options.Tool, out var tool))
        {
            Console.WriteLine("The specified tool could not be found.");
            return;
        }
        
        
        Console.WriteLine($"Starting {options.Tool} tool...");
        tool.Execute();
    }
}