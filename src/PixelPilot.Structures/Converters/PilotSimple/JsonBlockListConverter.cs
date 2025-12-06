using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using PixelPilot.Api;
using PixelPilot.Client.Messages;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;
using PixelPilot.Common.Logging;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.Migrations;

namespace PixelPilot.Structures.Converters.PilotSimple;

/// <summary>
/// Additional binding to convert a list of placed blocks to a save format.
/// This format also takes into consideration a version number.
/// </summary>
public class JsonBlockListConverter : JsonConverter<List<IPlacedBlock>>
{
    private static ILogger _logger = LogManager.GetLogger("JsonBlockListConverter");
    private static VersionManager _versionManager = new();
    
    public PixelApiClient _apiClient;

    public JsonBlockListConverter(PixelApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public override List<IPlacedBlock>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        var mappedBlockData = document.Deserialize<MappedBlockData>();
        if (mappedBlockData == null) return null;
        
        // Apply changes if required.
        _versionManager.ApplyMigrations(mappedBlockData);
        
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
            int x = -1;
            int y = -1;
            int layer = -1;
            int tempId = -1;
            PixelBlock block = PixelBlock.Empty;
            
            try
            {
                var bytes = new MemoryStream(Convert.FromBase64String(baseString));
                var binReader = new BinaryReader(bytes);

                x = binReader.ReadInt32();
                y = binReader.ReadInt32();
                layer = binReader.ReadInt32();
                
                // Mapping code
                tempId = binReader.ReadInt32();
                block = blockMapping[tempId];

                blocks.Add(new PlacedBlock(x, y, layer, LegacyStructureSerializer(binReader, block)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while serializing the entry: {BASE_STRING} (X: {X} Y: {Y} LAYER: {LAYER} TEMPID: {TEMP} BLOCK: {PIXEL_BLOCK})", baseString, x, y, layer, tempId, block);
                throw;
            }
        }

        return blocks;
    }
    
    
    public FlexBlock LegacyStructureSerializer(BinaryReader reader, PixelBlock block)
    {
        // We want to construct a PixelBlock.
        // First we need to know what type it is.
        // Then we can fill in the rest.
        var blockType = block.GetBlockType();
        var extraFields = blockType.GetPacketFieldTypes();

        // Read the extra fields
        var extra = extraFields.Select(fieldT => BinaryFieldConverter.ReadTypeLe(reader, fieldT)).ToList();

        // Now since it's not order based anymore we need to convert
        // from order to the correct names.
        // Well that sucks pretty hard you know.
        var webRequest = _apiClient.GetPixelBlockMeta();
        webRequest.Wait();

        var mappings = webRequest.Result;
        var mapping = mappings.FirstOrDefault(m => m.PaletteId == ToSnakeCase(block.ToString()));
        if (mapping == null)
        {
            throw new PixelApiException($"Could not find mapping for {block.ToString()}. Translated to: {ToSnakeCase(block.ToString())}");
        }
        
        
        var dict = new Dictionary<string, object>();
        for (int i = 0; i < mapping.Fields.Count; i++)
        {
            dict.Add(mapping.Fields[i].Name, extra[0]);
        }

        return new FlexBlock((int)block, dict);
    }
    
    private static Dictionary<string, string> SnakeCache = new Dictionary<string, string>();
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        if (SnakeCache.TryGetValue(input, out var snakeCase))
        {
            return snakeCase;
        }

        string result = input;

        // Insert underscore between lower-to-upper (e.g., "myVar" → "my_Var")
        result = Regex.Replace(result, "([a-z0-9])([A-Z])", "$1_$2");

        // Insert underscore between letter–number (e.g., "Value1" → "Value_1")
        result = Regex.Replace(result, "([A-Za-z])([0-9])", "$1_$2");
        
        result = result.ToLower();
        
        result = result.Replace("2_px", "2px");
        result = result.Replace("1_px", "1px");
        
        SnakeCache.Add(input, result);
        return result;
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