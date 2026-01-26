using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Structures.Converters.Pilot2;

namespace PixelPilot.Structures.Converters.FlexblockMigrations;

public abstract class Pilot2MigrationBase
{
    
    /// <summary>
    /// The version targeted by this migration.
    /// </summary>
    public int TargetVersion { get; set; }
    
    public string Description { get; set; }
    
    /// <summary>
    /// What version the FlexBlock will be after this change.
    /// </summary>
    public int ToVersion => TargetVersion + 1;
    
    public Pilot2MigrationBase(int targetVersion, string description)
    {
        TargetVersion = targetVersion;
        Description = description;
    }
    
    public virtual void Migrate(PilotStructureSave pilotStructureSave)
    {
        // Implemented as required. For example pallet changes can go here.
    }
}