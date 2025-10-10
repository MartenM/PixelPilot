using PixelPilot.Client.World.Constants;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_10_10B() : VersionMigration(11)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        const string GravityName = "EffectsGravityforce";
        
        const string Up = "EffectsGravityUp";
        const string Right = "EffectsGravityRight";
        const string Down = "EffectsGravityDown";
        const string Left = "EffectsGravityLeft";
        
        // Don't do anything if this struct does not contain spikes.
        if (!mappedBlockData.Mapping.Contains(GravityName)) return;

        int mappedSpikeId = mappedBlockData.Mapping.IndexOf(GravityName);
        
        // Replace the now 'old' name with Empty.
        mappedBlockData.Mapping[mappedSpikeId] = "Empty";
        
        // Add the new ID's.
        mappedBlockData.Mapping.AddRange([Up, Right, Down, Left]);
        int startIndex = mappedBlockData.Mapping.IndexOf(Up);
        
        for (int i = 0; i < mappedBlockData.BlockData.Count; i++)
        {
            using var bytes = new MemoryStream(Convert.FromBase64String(mappedBlockData.BlockData[i]));
            using var binReader = new BinaryReader(bytes);
            
            var x = binReader.ReadInt32();
            var y = binReader.ReadInt32();
            var layer = binReader.ReadInt32();
            
            var tempId = binReader.ReadInt32();
            if (tempId != mappedSpikeId) continue;

            int rotation = binReader.ReadInt32();
            
            // Welp lets change this a bit.
            var migratedBlock = Convert.ToBase64String(CreateSimpleBlock(x, y, layer, startIndex + rotation));
            
            mappedBlockData.BlockData[i] = migratedBlock;
        }
    }

    private byte[] CreateSimpleBlock(int x, int y, int layer, int customId)
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