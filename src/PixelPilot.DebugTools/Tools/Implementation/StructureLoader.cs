﻿using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.DebugTools.Tools.Implementation;

public class StructureLoader : CommandLineTool
{
    protected override void ExecuteCommand(string[] args, string fullText)
    {
        var location = fullText;
        var fileData = File.ReadAllText(location);

        var structure = PilotSaveSerializer.Deserialize(fileData);
        
        Console.WriteLine("The struct was loaded successfully!");
        Console.WriteLine($"Total blocks: \t{structure.Blocks.Count}");
        Console.WriteLine($"Dimensions: \t{structure.Width}x{structure.Height}");
    }
}