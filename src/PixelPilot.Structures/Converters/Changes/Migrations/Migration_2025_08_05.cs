using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_08_05() : VersionMigration(6)
{
    protected override void DoUpdate(MappedBlockData mappedBlockData)
    {
        for (int i = 0; i < mappedBlockData.Mapping.Count; i++)
        {
            mappedBlockData.Mapping[i] = UpdateName(mappedBlockData.Mapping[i]);
        }
    }

    private static string UpdateName(string old)
    {
        if (old.StartsWith("Gold"))
        {
            return old.Replace("Gold", "Gilded");
        }

        return old;
    }
}