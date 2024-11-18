using PixelPilot.Structures.Converters.Migrations.Migrations;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations;

public class VersionManager
{
    private readonly Dictionary<int, VersionMigration> _migrations = new();

    public VersionManager()
    {
        AddMigration(new Migration_2024_07_18());
        AddMigration(new Migration_2024_07_29());
        AddMigration(new Migration_2024_11_18());
    }

    private void AddMigration(VersionMigration migration)
    {
        _migrations.Add(migration.FromVersion, migration);
    }
    
    public void ApplyMigrations(MappedBlockData mappedBlockData)
    {
        VersionMigration? migration;

        while (_migrations.TryGetValue(mappedBlockData.Version, out migration))
        {
            migration.ApplyMigration(mappedBlockData);
        }
    }
}