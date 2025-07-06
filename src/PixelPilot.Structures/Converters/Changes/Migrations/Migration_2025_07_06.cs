using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_07_06() :  VersionMigration(7)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        const string NormalPortalName = "Portal";
        const string PortalInvisible = "PortalInvisible";

        List<string> oldPortalNames = new List<string>()
        {
            "Portal",
            "PortalInvisible",
        };
        
        var portalTempIds = new Dictionary<int, string>();
        foreach (var oldPortalName in oldPortalNames)
        {
            var tempId = mappedBlockData.Mapping.IndexOf(oldPortalName);
            if (tempId == -1) continue;
            
            portalTempIds.Add(tempId, oldPortalName);
            
            mappedBlockData.Mapping[tempId] = "Empty";
        }
        
        // Write new portal block data.
        string[] directions = new string[]
        {
            "Left",
            "Up",
            "Right",
            "Down",
        };

        if (portalTempIds.Count == 0) return;
        
        // Go over all blocks and modify entries where required.
        for (int i = mappedBlockData.BlockData.Count - 1; i >= 0; i--)
        {
            using var bytes = new MemoryStream(Convert.FromBase64String(mappedBlockData.BlockData[i]));
            using var binReader = new BinaryReader(bytes);
            
            var x = binReader.ReadInt32();
            var y = binReader.ReadInt32();
            var layer = binReader.ReadInt32();
            
            var tempId = binReader.ReadInt32();
            
            // If this is a portal block.
            if (!portalTempIds.TryGetValue(tempId, out var oldBlockName)) continue;

            int direction = binReader.ReadInt32();
            string portalId = binReader.ReadInt32().ToString();
            string targetId = binReader.ReadInt32().ToString();
            
            // Get the new name.
            var newName = $"{oldBlockName}{directions[direction]}";
            
            // Get or add the new name.
            var newId = mappedBlockData.Mapping.IndexOf(newName);
            if (newId == -1)
            {
                mappedBlockData.Mapping.Add(newName);
                
                // This should be size - 1 but EHHH.
                newId = mappedBlockData.Mapping.IndexOf(newName);
            }
            
            var migratedBlock = Convert.ToBase64String(CreatePortalBlock(x, y, layer, newId, portalId, targetId));
            mappedBlockData.BlockData[i] = migratedBlock;
        }
    }
    
    private byte[] CreatePortalBlock(int x, int y, int layer, int customId, string portalId, string targetId)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(portalId);
        writer.Write(targetId);

        return memoryStream.ToArray();
    }
}