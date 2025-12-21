using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2024_11_18() : VersionMigration(3)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        const string WorldPortalName = "PortalWorld";
        
        // Don't do anything if this struct does not contain world portal.
        if (!mappedBlockData.Mapping.Contains(WorldPortalName)) return;
        
        int mappedWorldPortal = mappedBlockData.Mapping.IndexOf(WorldPortalName);
        
        // Go over all blocks and modify entries where required.
        for (int i = mappedBlockData.BlockData.Count - 1; i >= 0; i--)
        {
            using var bytes = new MemoryStream(Convert.FromBase64String(mappedBlockData.BlockData[i]));
            using var binReader = new BinaryReader(bytes);
            
            var x = binReader.ReadInt32();
            var y = binReader.ReadInt32();
            var layer = binReader.ReadInt32();
            
            var tempId = binReader.ReadInt32();
            if (tempId != mappedWorldPortal) continue;

            string? worldId = bytes.Position != bytes.Length ? binReader.ReadString() : null;
            if (worldId == null || worldId.Length == 0)
            {
                // Remove this portal as it's invalid.
                mappedBlockData.BlockData.RemoveAt(i);
                continue;
            }
            
            // Now it's missing the spawn ID
            
            // Welp lets change this a bit.
            var migratedBlock = Convert.ToBase64String(CreateWorldPortalBlock(x, y, layer, tempId, worldId));
            
            mappedBlockData.BlockData[i] = migratedBlock;
        }
    }
    
    private byte[] CreateWorldPortalBlock(int x, int y, int layer, int customId, string worldId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(worldId);
        writer.Write((int) 0);

        return memoryStream.ToArray();
    }
}