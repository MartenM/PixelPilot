using System.Drawing;
using Google.Protobuf;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Zones;

public interface IZone
{
    public string Name { get; }
    public int Priority { get; }

    /// <summary>
    /// Display color as a hue (0-359).
    /// </summary>
    public int Hue { get; }

    public int Width { get; }
    public int Height { get; }

    /// <summary>
    /// Per-block membership mask, indexed [x, y].
    /// </summary>
    public bool[,] Membership { get; }

    /// <summary>
    /// The number of blocks that are a member of this zone.
    /// </summary>
    public int Size { get; }

    public ZoneVisionState Vision { get; }

    /// <summary>
    /// Hides the zone's interior from players outside it.
    /// </summary>
    public ZoneVisionState VisionOutside { get; }

    public bool HasVisionColor { get; }

    /// <summary>
    /// Fill color for hidden areas; overrides the world's void color.
    /// </summary>
    public Color VisionColor { get; }

    public ZoneCameraModeState CameraModeX { get; }
    public ZoneCameraModeState CameraModeY { get; }
    public ZoneCameraTargetState CameraTarget { get; }
    public ZoneCameraMovementState CameraMovement { get; }

    /// <summary>
    /// Camera follow smoothing while not locked to this zone.
    /// </summary>
    public ZoneCameraMovementState CameraFollowMovement { get; }

    public ZoneLightingState Lighting { get; }
    public int LightDarkness { get; }
    public int LightHue { get; }
    public int LightTint { get; }
    public int LightFeatherTop { get; }
    public int LightFeatherRight { get; }
    public int LightFeatherBottom { get; }
    public int LightFeatherLeft { get; }
    public int LightMarginTop { get; }
    public int LightMarginRight { get; }
    public int LightMarginBottom { get; }
    public int LightMarginLeft { get; }
    public int LightSmoothing { get; }

    /// <summary>
    /// Personal light carried by players inside the zone.
    /// </summary>
    public ZonePlayerLightState PlayerLight { get; }
    public int PlayerLightRadius { get; }
    public int PlayerLightStrength { get; }
    public int PlayerLightHue { get; }
    public int PlayerLightSaturation { get; }

    public ZoneFogState Fog { get; }
    public int FogHue { get; }
    public int FogSaturation { get; }
    public int FogOpacity { get; }
    public int FogDensity { get; }
    public int FogDirection { get; }
    public int FogSpeed { get; }
    public int FogFeatherTop { get; }
    public int FogFeatherRight { get; }
    public int FogFeatherBottom { get; }
    public int FogFeatherLeft { get; }
    public int FogMarginTop { get; }
    public int FogMarginRight { get; }
    public int FogMarginBottom { get; }
    public int FogMarginLeft { get; }
    public int FogSmoothing { get; }

    public ZoneDistortionState Distortion { get; }
    public int DistortionStrength { get; }
    public int DistortionScale { get; }
    public int DistortionSpeed { get; }
    public int DistortionFeatherTop { get; }
    public int DistortionFeatherRight { get; }
    public int DistortionFeatherBottom { get; }
    public int DistortionFeatherLeft { get; }
    public int DistortionMarginTop { get; }
    public int DistortionMarginRight { get; }
    public int DistortionMarginBottom { get; }
    public int DistortionMarginLeft { get; }
    public int DistortionSmoothing { get; }

    public ProtoZone AsProtoZone();

    /// <summary>
    /// Builds a create/update-settings request. Pass an existing zone's id to update its
    /// settings; omit it to create a new zone (server-assigned id). The server ignores
    /// Width/Height/MembershipRle on this packet — membership can only be changed via area-edit
    /// requests (see <see cref="ZoneMembershipRects"/>).
    /// </summary>
    public IMessage ToUpsertPacket(string? id = null);

    /// <summary>
    /// Compares every setting except <see cref="Membership"/> (and its Width/Height, which only
    /// describe the membership grid's dimensions).
    /// </summary>
    public bool SettingsEqual(IZone other);
}
