using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Structures.Converters.Pilot2;

namespace PixelPilot.Structures.Converters.FlexblockMigrations.Migrations;

public class Migration_2025_01_26_GlowLines : Pilot2MigrationBase
{
    public Migration_2025_01_26_GlowLines() : base(20, "Adds the HEX part to the BorderGlowLine blocks. Also minor patch for world portals.")
    {
        
    }

    public override void Migrate(PilotStructureSave save)
    {
        base.Migrate(save);

        foreach (var palletMapping in save.BlockPallet)
        {
            if (palletMapping.Name.StartsWith("BorderGlow"))
            {
                palletMapping.Fields.Add("color", (uint) 16777215);
            }
            if (palletMapping.Name.StartsWith("BorderOutline"))
            {
                palletMapping.Fields.Add("color", (uint) 16777215);
            }

            if (palletMapping.Name.Equals("PortalWorld"))
            {
                // This is kinda 'drastic'. But due to permission issues portals don't properly place.
                palletMapping.Fields.Remove("spawn_id");
                palletMapping.Fields.Add("spawn_id", "");
            }
        }
    }
}