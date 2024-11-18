using PixelPilot.PixelGameClient.World.Constants;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2024_07_29() : VersionMigration(2)
{
    
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        const string SpikesName = "Spikes";

        const string SpikeUp = "HazardSpikesUp";
        const string SpikeRight = "HazardSpikesRight";
        const string SpikeDown = "HazardSpikesDown";
        const string SpikeLeft = "HazardSpikesLeft";
        
        // Don't do anything if this struct does not contain spikes.
        if (!mappedBlockData.Mapping.Contains(SpikesName)) return;

        int mappedSpikeId = mappedBlockData.Mapping.IndexOf(SpikesName);
        
        // Replace the now 'old' name with Empty.
        mappedBlockData.Mapping[mappedSpikeId] = "Empty";
        
        // Add the new ID's.
        mappedBlockData.Mapping.AddRange([SpikeUp, SpikeRight, SpikeDown, SpikeLeft]);
        int startIndex = mappedBlockData.Mapping.IndexOf(SpikeUp);
        
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