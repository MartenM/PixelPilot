using PixelPilot.Client.World.Constants;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_10_10C() : VersionMigration(11)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        const string JumpName = "EffectsJumpHeight";
        
        // Don't do anything if this struct does not contain spikes.
        if (!mappedBlockData.Mapping.Contains(JumpName)) return;
        int mappedSpikeId = mappedBlockData.Mapping.IndexOf(JumpName);
        
        for (int i = 0; i < mappedBlockData.BlockData.Count; i++)
        {
            using var bytes = new MemoryStream(Convert.FromBase64String(mappedBlockData.BlockData[i]));
            using var binReader = new BinaryReader(bytes);
            
            var x = binReader.ReadInt32();
            var y = binReader.ReadInt32();
            var layer = binReader.ReadInt32();
            
            var tempId = binReader.ReadInt32();
            if (tempId != mappedSpikeId) continue;

            int jumpHeight = binReader.ReadInt32();
            var percentage = jumpHeight switch
            {
                1 => 67, // 0.666
                2 => 78, // 0.777
                3 => 100,
                4 => 111, // 1.111
                5 => 122, // 1.222
                6 => 133, // 1.333
                7 => 144, // 1.444
                8 => 156, // 1.555
                9 => 167, // 1.666
                _ => 100
            };
            
            
            // Welp lets change this a bit.
            // PIXELWALKER DOES NOT ALLOW VALUES THAT ARE NOT 5 % = 0
            var migratedBlock = Convert.ToBase64String(CreateSimpleBlock(x, y, layer, mappedSpikeId, (int) Math.Round(percentage / 5.0) * 5));
            
            mappedBlockData.BlockData[i] = migratedBlock;
        }
    }

    private byte[] CreateSimpleBlock(int x, int y, int layer, int customId, int percentage)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);
        
        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(percentage);

        return memoryStream.ToArray();
    }

}