namespace PixelPilot.DebugTools.Tools;

public abstract class CommandLineTool : IDebugTool
{

    public virtual string BeforeEach { get; } = "CL: ";
    public void Execute()
    {
        while (true)
        {
            Console.Write(BeforeEach);
            
            var line = Console.ReadLine();
            if (line == null) break;

            var split = line.Split(" ");
            if (split[0].ToLower().Equals("stop"))
            {
                break;
            }
            
            ExecuteCommand(split, line);
        }
    }

    protected abstract void ExecuteCommand(string[] args, string fullText);
}