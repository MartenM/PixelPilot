using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_08_18() : VersionMigration(9)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        int tempSpaceSignId = -1;
        for (int i = 0; i < mappedBlockData.Mapping.Count; i++)
        {
            var entry = mappedBlockData.Mapping[i];
            if (entry.Equals("OuterspaceSign"))
            {
                mappedBlockData.Mapping[i] = "OuterspaceSignGreen";
                tempSpaceSignId = i;
                break;
            }
        }

        if (tempSpaceSignId == -1)
        {
            return;
        }

        // Go over all entries and modify where required.
        for (int i = mappedBlockData.BlockData.Count - 1; i >= 0; i--)
        {
            using var bytes = new MemoryStream(Convert.FromBase64String(mappedBlockData.BlockData[i]));
            using var binReader = new BinaryReader(bytes);

            var x = binReader.ReadInt32();
            var y = binReader.ReadInt32();
            var layer = binReader.ReadInt32();

            var tempId = binReader.ReadInt32();

            // If this is a portal block.
            if (tempId != tempSpaceSignId) continue;

            var migratedBlock = Convert.ToBase64String(CreateSignBlock(x, y, layer, tempId, ""));
            mappedBlockData.BlockData[i] = migratedBlock;
        }
    }

    private byte[] CreateSignBlock(int x, int y, int layer, int customId, string text)
    {
        using MemoryStream memoryStream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(memoryStream);

        writer.Write(x);
        writer.Write(y);
        writer.Write(layer);
        writer.Write(customId);
        writer.Write(text);

        return memoryStream.ToArray();
    }

}