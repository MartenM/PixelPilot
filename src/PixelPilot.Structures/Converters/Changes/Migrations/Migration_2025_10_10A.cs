using PixelPilot.Client.World.Constants;
using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2025_10_10A() : VersionMigration(10)
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
        return old switch
        {
            "BeachRock" => "BeachRockGraySmall",
            "OuterspaceRockGray" => "BeachRockGrayLarge",
            "ScifiLaserOrangeStraightVeritical" => "ScifiLaserOrangeStraightVertical",
            "SummerPlankYelllow" => "SummerPlankYellow",
            "SummerIceCreamSrawberry" => "SummerIceCreamStrawberry",
            "DomesticBottomRight" => "DomesticPipeBottomRight",
            "EffectsGravityforce" => "EffectsGravityForce",
            "MineStalagtite" => "MineStalactite",
            _ => old
        };
    }

}