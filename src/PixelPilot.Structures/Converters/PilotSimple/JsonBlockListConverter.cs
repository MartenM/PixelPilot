using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using PixelPilot.Common.Logging;
using PixelPilot.PixelGameClient.World;
using PixelPilot.PixelGameClient.World.Blocks.Placed;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.Structures.Converters.PilotSimple;

/// <summary>
/// Additional binding to convert a list of placed blocks to a save format.
/// This format also takes into consideration a version number.
/// </summary>
public class JsonBlockListConverter : JsonConverter<List<IPlacedBlock>>
{
    private static ILogger _logger = LogManager.GetLogger("JsonBlockListConverter");

    public int MappingsVersion;

    public override List<IPlacedBlock>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        var mappedBlockData = document.Deserialize<MappedBlockData>();
        if (mappedBlockData == null) return null;
        
        // Setup dict for easy access
        Dictionary<int, PixelBlock> blockMapping = new();
        for (int i = 0; i < mappedBlockData.Mapping.Count; i++)
        {
            var entry = mappedBlockData.Mapping[i];
            blockMapping.Add(i,  (PixelBlock) Enum.Parse(typeof(PixelBlock), entry));
        }
        
        // Now deconstruct the blocks. Yay
        List<IPlacedBlock> blocks = new();
        foreach (var baseString in mappedBlockData.BlockData)
        {
            try
            {
                var bytes = new MemoryStream(Convert.FromBase64String(baseString));
                var binReader = new BinaryReader(bytes);

                var x = binReader.ReadInt32();
                var y = binReader.ReadInt32();
                var layer = binReader.ReadInt32();
                
                // Mapping code
                var tempId = binReader.ReadInt32();
                var block = blockMapping[tempId];

                blocks.Add(new PlacedBlock(x, y, layer, PixelWorld.DeserializeBlock(binReader, block)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong while serializing the entry: {baseString}");
            }
        }

        return blocks;
    }

    public override void Write(Utf8JsonWriter writer, List<IPlacedBlock> blocks, JsonSerializerOptions options)
    {
        var currentIndex = 0;
        
        // Fast lookup
        Dictionary<PixelBlock, int> indexDict = new();
        List<PixelBlock> mapping = new();

        List<string> blocksData = new();
        
        foreach (var block in blocks)
        {
            // Check if the block has already been mapped.
            // If so save it as temp id.
            if (indexDict.TryGetValue(block.Block.Block, out var tempId))
            {
                // Save it according to it's temp ID.
                blocksData.Add(Convert.ToBase64String(block.AsWorldBuffer(tempId)));
                continue;
            }
            
            // Otherwise, generate a new id.
            var newId = currentIndex;
            currentIndex++;
            
            indexDict.Add(block.Block.Block, newId);
            mapping.Add(block.Block.Block);
            
            blocksData.Add(Convert.ToBase64String(block.AsWorldBuffer(newId)));
        }

        JsonSerializer.Serialize(writer, new MappedBlockData(mapping, blocksData), options);
    }
}