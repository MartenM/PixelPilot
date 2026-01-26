using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Structures.Converters.FlexblockMigrations.Migrations;
using PixelPilot.Structures.Converters.Pilot2;

namespace PixelPilot.Structures.Converters.FlexblockMigrations;

public class Pilot2MigrationManager
{
    
    private List<Pilot2MigrationBase> Pilot2Migrations { get; set; }
    
    public Pilot2MigrationManager()
    {
        Pilot2Migrations = new List<Pilot2MigrationBase>()
        {
            new Migration_2025_01_26_GlowLines()
        };
    }

    private Pilot2MigrationBase? GetAvailableMigration(int version)
    {
        return Pilot2Migrations.FirstOrDefault(m => m.TargetVersion == version);
    }

    public void Migrate(PilotStructureSave save)
    {
        var originalVersion = save.Version;
        
        var migration = GetAvailableMigration(save.Version);
        while (migration != null)
        {
            migration.Migrate(save);
            save.Version = migration.ToVersion;
            
            migration = GetAvailableMigration(save.Version);
        }
    }
}