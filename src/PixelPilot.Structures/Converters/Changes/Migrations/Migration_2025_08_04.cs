using PixelPilot.Client.World.Blocks;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_08_04() : VersionMigration(5)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        var overlayBlocks = new List<string>()
        {
            "LiquidWater",
            "LiquidWaterSurface",
            "LiquidWaste",
            "LiquidWasteSurface",
            "LiquidLava",
            "LiquidLavaSurface",
            "LiquidMud",
            "LiquidMudSurface",
        };
        
        // Convert the nampes to custom ids.
        var customIdList = overlayBlocks.Select(b => mappedBlockData.Mapping.IndexOf(b))
                                        .Where(id => id >= 0)
                                        .ToList();
        
        // Don't do anything if this struct does not contain the blocks.
        if (!customIdList.Any()) return;
        
        // Go over all blocks and modify entries where required.
        for (int i = mappedBlockData.BlockData.Count - 1; i >= 0; i--)
        {
            using var bytes = new MemoryStream(Convert.FromBase64String(mappedBlockData.BlockData[i]));
            using var binReader = new BinaryReader(bytes);
            
            var x = binReader.ReadInt32();
            var y = binReader.ReadInt32();
            var layer = binReader.ReadInt32();
            
            var tempId = binReader.ReadInt32();
            if (!customIdList.Contains(tempId)) continue;

            
            mappedBlockData.BlockData[i] = Convert.ToBase64String(CreateNormalBlock(x, y, 2, tempId));
        }
    }
    
    private byte[] CreateNormalBlock(int x, int y, int layer, int customId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);

        return memoryStream.ToArray();
    }
}