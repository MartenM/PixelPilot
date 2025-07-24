using PixelPilot.Client.World.Constants;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_07_24() : VersionMigration(8)
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
        if (old.Equals("LeprechaunGoldBag")) return "PirateGoldBag";
        
        // Ignore if the palette name is not hazard_spikes_direction.
        if (!old.StartsWith("Glow"))
            return old;
        
        return old switch
        {
            "GlowLineLeftPointTopLeftBottomRight" => "BorderGlowLineLeftPointTopRightBottomRight",
            "GlowStraightVertical" => "BorderGlowStraightHorizontal",
            "GlowStraightHorizontal" => "BrderGlowStraightVertical",
            _ => "Border" + old // Keep the rest of the name unchanged
        };
    }
}