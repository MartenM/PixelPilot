using PixelPilot.Structures.Converters.Changes;
using PixelPilot.Structures.Converters.PilotSimple;

namespace PixelPilot.Structures.Converters.Migrations.Migrations;

public class Migration_2024_12_04() : VersionMigration(4)
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
			"HazardSpikesUp" => "HazardSpikesBrownUp",
			"HazardSpikesRight" => "HazardSpikesBrownRight",
			"HazardSpikesDown" => "HazardSpikesBrownDown",
			"HazardSpikesLeft" => "HazardSpikesBrownLeft",
			"HazardSpikesCenter" => "HazardSpikesBrownCenter",
			_ => old
		};
	}
}