using System.Text.Json;
using PixelPilot.Api;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Pilot2;

public static class StructureExtensions
{
    /// <summary>
    /// Convert a structure to the save format.
    /// </summary>
    /// <param name="structure"></param>
    /// <returns></returns>
    public static PilotStructureSave ToSave(this Structure structure)
    {
        var save = new PilotStructureSave();
        save.Height =  structure.Height;
        save.Width =  structure.Width;
        save.Meta =  structure.Meta;

        save.BlocksVersion = 1;
        
        // Create mappings
        var createMapping = new Dictionary<PalletMapping, int>();
        var mappedBlocks = new List<PalletReference>();
        foreach (var block in structure.Blocks)
        {
            var mapingId = MapOrGet(createMapping, (FlexBlock) block.Block);
            mappedBlocks.Add(new PalletReference()
            {
                Layer =  block.Layer,
                X =  block.X,
                Y = block.Y,
                PalletIndex =  mapingId
            });
        }

        // Set mappings.
        save.BlockPallet = new List<PalletMapping>(mappedBlocks.Count);
        foreach (var map in createMapping)
        {
            save.BlockPallet.Insert(map.Value, map.Key);
        }
        save.BlockReferences = mappedBlocks;

        return save;
    }

    private static int MapOrGet(Dictionary<PalletMapping, int> mapping, FlexBlock flexBlock)
    {
        var map = new PalletMapping()
        {
            Name = flexBlock.Block.ToString(),
            Fields = new Dictionary<string, object>(flexBlock.Fields)
        };

        if (mapping.TryGetValue(map, out int index))
        {
            return index;
        }

        var newIndex = mapping.Count;
        mapping.Add(map, newIndex);
        
        return newIndex;
    }
    

    /// <summary>
    /// Convert a save format back to a structure.
    /// </summary>
    /// <param name="save"></param>
    /// <returns></returns>
    public static Structure ToStructure(this PilotStructureSave save)
    {
        var blocks = new List<IPlacedBlock>();

        foreach (var blockRef in save.BlockReferences)
        {
            var palletData = save.BlockPallet[blockRef.PalletIndex];
            var bock = (PixelBlock) Enum.Parse(typeof(PixelBlock), palletData.Name);
            var blockData = new FlexBlock((int) bock, new Dictionary<string, object>(palletData.Fields));
            
            blocks.Add(new PlacedBlock(blockRef.X, blockRef.Y, blockRef.Layer, blockData));
        }
        
        var structure = new Structure(
            save.Width, save.Height, save.Meta, false, blocks);

        return structure;
    }
    
    /// <summary>
    /// Convert a structure to the JSON save format.
    /// </summary>
    /// <param name="structure"></param>
    /// <param name="filePath"></param>
    /// <param name="writeIndented"></param>
    public static async Task ToFile(this Structure structure, string filePath, bool writeIndented = false)
    {
        var saveText = structure.ToSave().ToJson(writeIndented);
        
        if (!filePath.EndsWith(".json"))
        {
            filePath += ".json";
        }

        await File.WriteAllTextAsync(filePath, saveText);
    }

    /// <summary>
    /// Convert a save format to JSON.
    /// </summary>
    /// <param name="save"></param>
    /// <param name="writeIndented"></param>
    /// <returns></returns>
    public static string ToJson(this PilotStructureSave save, bool writeIndented)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = writeIndented,
            Converters =
            {
                new PalletReferenceConverter()
            }
        };

        var text = JsonSerializer.Serialize(save, options);
        return text;
    }

    public static string ToJson(this Structure structure, bool writeIndented = false)
    {
        return structure.ToSave().ToJson(writeIndented);
    }

    /// <summary>
    /// Load a structure from a file in save format.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="writeIndented"></param>
    /// <returns></returns>
    /// <exception cref="PixelApiException"></exception>
    public static async Task<Structure> LoadFile(string filePath, bool writeIndented = false)
    {
        if (!filePath.EndsWith(".json"))
        {
            filePath += ".json";
        }

        if (!File.Exists(filePath))
        {
            throw new PixelApiException("Could not find file: " + filePath);
        }

        var saveText = await File.ReadAllTextAsync(filePath);
        var save = GetPilotStructureSave(saveText);

        return save.ToStructure();
    }

    /// <summary>
    /// Get the safe format from a save.
    /// </summary>
    /// <param name="saveText"></param>
    /// <returns></returns>
    /// <exception cref="PixelApiException"></exception>
    public static PilotStructureSave GetPilotStructureSave(string saveText)
    {
        var save = JsonSerializer.Deserialize<PilotStructureSave>(saveText, options: new JsonSerializerOptions()
        {
            Converters = { 
                new PalletReferenceConverter() 
            }
        }) ?? throw new PixelApiException("Could load structure");

        // UInt32 does a bit weird, but it will behave like this.
        // Could maybe do some checking here too.
        foreach (var pallet in save.BlockPallet)
        {
            if (pallet.Fields.Count == 0)
            {
                continue;
            }

            foreach (var kvp in pallet.Fields)
            {
                if (kvp.Value is JsonElement e)
                {
                    if (e.ValueKind == JsonValueKind.Number)
                    {
                        pallet.Fields[kvp.Key] = e.GetUInt32();
                    }
                }
            }
        }

        // Safety check so no virus gets out ;)
        if (save.BlockPallet.Any(p => p.Fields?.Any(kvp => kvp.Value is JsonElement) == true))
        {
            throw new PixelApiException("A field was not properly converted to it's native type.");
        }
        
        return save;
    }
}