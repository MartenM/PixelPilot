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

            try
            {
                ExecuteCommand(split, line);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing the CL Tool");
                Console.WriteLine(ex);
            }
        }
    }

    protected abstract void ExecuteCommand(string[] args, string fullText);
}