using System.Drawing;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Labels;
using PixelPilot.Client.World.Zones;
using PixelPilot.Structures;
using PixelPilot.Structures.Converters.Pilot2;

namespace PixelGameTests.Structures;

public class ZoneStructureTests
{
    private static Zone BuildSampleZone()
    {
        var membership = new bool[5, 5];
        for (var x = 1; x < 4; x++)
        for (var y = 1; y < 4; y++)
            membership[x, y] = true;

        return new Zone
        {
            Name = "Test Zone",
            Priority = 3,
            Hue = 210,
            Width = 5,
            Height = 5,
            Membership = membership,

            Vision = ZoneVisionState.On,
            VisionOutside = ZoneVisionState.Off,
            HasVisionColor = true,
            VisionColor = Color.FromArgb(255, 20, 40, 60),

            CameraModeX = ZoneCameraModeState.Lock,
            CameraModeY = ZoneCameraModeState.Follow,
            CameraTarget = ZoneCameraTargetState.Part,
            CameraMovement = ZoneCameraMovementState.Smooth,
            CameraFollowMovement = ZoneCameraMovementState.Instant,

            Lighting = ZoneLightingState.On,
            LightDarkness = 50,
            LightHue = 180,
            LightTint = 25,
            LightFeatherTop = 1,
            LightFeatherRight = 2,
            LightFeatherBottom = 3,
            LightFeatherLeft = 4,
            LightMarginTop = 5,
            LightMarginRight = 6,
            LightMarginBottom = 7,
            LightMarginLeft = 8,
            LightSmoothing = 9,

            PlayerLight = ZonePlayerLightState.On,
            PlayerLightRadius = 100,
            PlayerLightStrength = 60,
            PlayerLightHue = 90,
            PlayerLightSaturation = 70,

            Fog = ZoneFogState.On,
            FogHue = 15,
            FogSaturation = 55,
            FogOpacity = 65,
            FogDensity = 75,
            FogDirection = 45,
            FogSpeed = 10,
            FogFeatherTop = 11,
            FogFeatherRight = 12,
            FogFeatherBottom = 13,
            FogFeatherLeft = 14,
            FogMarginTop = 15,
            FogMarginRight = 16,
            FogMarginBottom = 17,
            FogMarginLeft = 18,
            FogSmoothing = 19,

            Distortion = ZoneDistortionState.On,
            DistortionStrength = 20,
            DistortionScale = 30,
            DistortionSpeed = 40,
            DistortionFeatherTop = 21,
            DistortionFeatherRight = 22,
            DistortionFeatherBottom = 23,
            DistortionFeatherLeft = 24,
            DistortionMarginTop = 25,
            DistortionMarginRight = 26,
            DistortionMarginBottom = 27,
            DistortionMarginLeft = 28,
            DistortionSmoothing = 29,
        };
    }

    [Test]
    public void TestZoneRoundTripsThroughPilot2Json()
    {
        var zone = BuildSampleZone();
        var structure = new Structure(5, 5, new Dictionary<string, string>(), true, new List<IPlacedBlock>(),
            new List<ITextLabel>(), new List<IZone> { zone });

        var json = Pilot2Serializer.ToJson(structure);
        var roundTripped = Pilot2Serializer.ToStructure(json);

        Assert.That(roundTripped.Zones, Has.Count.EqualTo(1));
        var roundTrippedZone = roundTripped.Zones[0];

        Assert.That(roundTrippedZone.Equals(zone), Is.True, "Round-tripped zone should equal the original.");
    }

    [Test]
    public void TestZoneMembershipRoundTripsThroughPilot2Json()
    {
        var zone = BuildSampleZone();
        var structure = new Structure(5, 5, new Dictionary<string, string>(), true, new List<IPlacedBlock>(),
            new List<ITextLabel>(), new List<IZone> { zone });

        var json = Pilot2Serializer.ToJson(structure);
        var roundTripped = Pilot2Serializer.ToStructure(json);
        var roundTrippedZone = roundTripped.Zones[0];

        for (var x = 0; x < zone.Width; x++)
        {
            for (var y = 0; y < zone.Height; y++)
            {
                Assert.That(roundTrippedZone.Membership[x, y], Is.EqualTo(zone.Membership[x, y]),
                    $"Membership mismatch at ({x},{y})");
            }
        }
    }

    [Test]
    public void TestStructureWithoutZonesRoundTrips()
    {
        var structure = new Structure(2, 2, new Dictionary<string, string>(), true, new List<IPlacedBlock>(),
            new List<ITextLabel>());

        var json = Pilot2Serializer.ToJson(structure);
        var roundTripped = Pilot2Serializer.ToStructure(json);

        Assert.That(roundTripped.Zones, Is.Empty);
    }
}
