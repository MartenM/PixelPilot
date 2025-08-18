using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_08_18() : VersionMigration(9)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        for (int i = 0; i < mappedBlockData.Mapping.Count; i++)
        {
            var entry = mappedBlockData.Mapping[i];
            if (entry.Equals("OuterspaceSign"))
            {
                mappedBlockData.Mapping[i] = "OuterspaceSignGreen";
            }
        }
    }
}