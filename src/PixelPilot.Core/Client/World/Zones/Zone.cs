using System.Drawing;
using Google.Protobuf;
using PixelPilot.Client.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Zones;

public class Zone : IZone
{
    public string Name { get; set; } = "";
    public int Priority { get; set; }
    public int Hue { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public bool[,] Membership { get; set; } = new bool[0, 0];

    /// <summary>
    /// The number of blocks that are a member of this zone.
    /// </summary>
    public int Size
    {
        get
        {
            var count = 0;
            for (var x = 0; x < Membership.GetLength(0); x++)
            for (var y = 0; y < Membership.GetLength(1); y++)
                if (Membership[x, y]) count++;

            return count;
        }
    }

    public ZoneVisionState Vision { get; set; }
    public ZoneVisionState VisionOutside { get; set; }
    public bool HasVisionColor { get; set; }
    public Color VisionColor { get; set; }

    public ZoneCameraModeState CameraModeX { get; set; }
    public ZoneCameraModeState CameraModeY { get; set; }
    public ZoneCameraTargetState CameraTarget { get; set; }
    public ZoneCameraMovementState CameraMovement { get; set; }
    public ZoneCameraMovementState CameraFollowMovement { get; set; }

    public ZoneLightingState Lighting { get; set; }
    public int LightDarkness { get; set; }
    public int LightHue { get; set; }
    public int LightTint { get; set; }
    public int LightFeatherTop { get; set; }
    public int LightFeatherRight { get; set; }
    public int LightFeatherBottom { get; set; }
    public int LightFeatherLeft { get; set; }
    public int LightMarginTop { get; set; }
    public int LightMarginRight { get; set; }
    public int LightMarginBottom { get; set; }
    public int LightMarginLeft { get; set; }
    public int LightSmoothing { get; set; }

    public ZonePlayerLightState PlayerLight { get; set; }
    public int PlayerLightRadius { get; set; }
    public int PlayerLightStrength { get; set; }
    public int PlayerLightHue { get; set; }
    public int PlayerLightSaturation { get; set; }

    public ZoneFogState Fog { get; set; }
    public int FogHue { get; set; }
    public int FogSaturation { get; set; }
    public int FogOpacity { get; set; }
    public int FogDensity { get; set; }
    public int FogDirection { get; set; }
    public int FogSpeed { get; set; }
    public int FogFeatherTop { get; set; }
    public int FogFeatherRight { get; set; }
    public int FogFeatherBottom { get; set; }
    public int FogFeatherLeft { get; set; }
    public int FogMarginTop { get; set; }
    public int FogMarginRight { get; set; }
    public int FogMarginBottom { get; set; }
    public int FogMarginLeft { get; set; }
    public int FogSmoothing { get; set; }

    public ZoneDistortionState Distortion { get; set; }
    public int DistortionStrength { get; set; }
    public int DistortionScale { get; set; }
    public int DistortionSpeed { get; set; }
    public int DistortionFeatherTop { get; set; }
    public int DistortionFeatherRight { get; set; }
    public int DistortionFeatherBottom { get; set; }
    public int DistortionFeatherLeft { get; set; }
    public int DistortionMarginTop { get; set; }
    public int DistortionMarginRight { get; set; }
    public int DistortionMarginBottom { get; set; }
    public int DistortionMarginLeft { get; set; }
    public int DistortionSmoothing { get; set; }

    public Zone() { }

    public Zone(IZone zone)
    {
        Name = zone.Name;
        Priority = zone.Priority;
        Hue = zone.Hue;
        Width = zone.Width;
        Height = zone.Height;
        Membership = (bool[,]) zone.Membership.Clone();

        Vision = zone.Vision;
        VisionOutside = zone.VisionOutside;
        HasVisionColor = zone.HasVisionColor;
        VisionColor = zone.VisionColor;

        CameraModeX = zone.CameraModeX;
        CameraModeY = zone.CameraModeY;
        CameraTarget = zone.CameraTarget;
        CameraMovement = zone.CameraMovement;
        CameraFollowMovement = zone.CameraFollowMovement;

        Lighting = zone.Lighting;
        LightDarkness = zone.LightDarkness;
        LightHue = zone.LightHue;
        LightTint = zone.LightTint;
        LightFeatherTop = zone.LightFeatherTop;
        LightFeatherRight = zone.LightFeatherRight;
        LightFeatherBottom = zone.LightFeatherBottom;
        LightFeatherLeft = zone.LightFeatherLeft;
        LightMarginTop = zone.LightMarginTop;
        LightMarginRight = zone.LightMarginRight;
        LightMarginBottom = zone.LightMarginBottom;
        LightMarginLeft = zone.LightMarginLeft;
        LightSmoothing = zone.LightSmoothing;

        PlayerLight = zone.PlayerLight;
        PlayerLightRadius = zone.PlayerLightRadius;
        PlayerLightStrength = zone.PlayerLightStrength;
        PlayerLightHue = zone.PlayerLightHue;
        PlayerLightSaturation = zone.PlayerLightSaturation;

        Fog = zone.Fog;
        FogHue = zone.FogHue;
        FogSaturation = zone.FogSaturation;
        FogOpacity = zone.FogOpacity;
        FogDensity = zone.FogDensity;
        FogDirection = zone.FogDirection;
        FogSpeed = zone.FogSpeed;
        FogFeatherTop = zone.FogFeatherTop;
        FogFeatherRight = zone.FogFeatherRight;
        FogFeatherBottom = zone.FogFeatherBottom;
        FogFeatherLeft = zone.FogFeatherLeft;
        FogMarginTop = zone.FogMarginTop;
        FogMarginRight = zone.FogMarginRight;
        FogMarginBottom = zone.FogMarginBottom;
        FogMarginLeft = zone.FogMarginLeft;
        FogSmoothing = zone.FogSmoothing;

        Distortion = zone.Distortion;
        DistortionStrength = zone.DistortionStrength;
        DistortionScale = zone.DistortionScale;
        DistortionSpeed = zone.DistortionSpeed;
        DistortionFeatherTop = zone.DistortionFeatherTop;
        DistortionFeatherRight = zone.DistortionFeatherRight;
        DistortionFeatherBottom = zone.DistortionFeatherBottom;
        DistortionFeatherLeft = zone.DistortionFeatherLeft;
        DistortionMarginTop = zone.DistortionMarginTop;
        DistortionMarginRight = zone.DistortionMarginRight;
        DistortionMarginBottom = zone.DistortionMarginBottom;
        DistortionMarginLeft = zone.DistortionMarginLeft;
        DistortionSmoothing = zone.DistortionSmoothing;
    }

    public static Zone FromProtoZone(ProtoZone protoZone)
    {
        var zone = new Zone();
        zone.UpdateWithProtoZone(protoZone);
        return zone;
    }

    public void UpdateWithProtoZone(ProtoZone zone)
    {
        Name = zone.Name;
        Priority = zone.Priority;
        Hue = zone.Hue;
        Width = zone.Width;
        Height = zone.Height;
        Membership = MembershipRle.Decode(zone.MembershipRle, zone.Width, zone.Height);

        Vision = zone.Vision.ToZoneVisionState();
        VisionOutside = zone.VisionOutside.ToZoneVisionState();
        HasVisionColor = zone.HasVisionColor;
        VisionColor = zone.VisionColor.ToColor();

        CameraModeX = zone.CameraModeX.ToZoneCameraModeState();
        CameraModeY = zone.CameraModeY.ToZoneCameraModeState();
        CameraTarget = zone.CameraTarget.ToZoneCameraTargetState();
        CameraMovement = zone.CameraMovement.ToZoneCameraMovementState();
        CameraFollowMovement = zone.CameraFollowMovement.ToZoneCameraMovementState();

        Lighting = zone.Lighting.ToZoneLightingState();
        LightDarkness = zone.LightDarkness;
        LightHue = zone.LightHue;
        LightTint = zone.LightTint;
        LightFeatherTop = zone.LightFeatherTop;
        LightFeatherRight = zone.LightFeatherRight;
        LightFeatherBottom = zone.LightFeatherBottom;
        LightFeatherLeft = zone.LightFeatherLeft;
        LightMarginTop = zone.LightMarginTop;
        LightMarginRight = zone.LightMarginRight;
        LightMarginBottom = zone.LightMarginBottom;
        LightMarginLeft = zone.LightMarginLeft;
        LightSmoothing = zone.LightSmoothing;

        PlayerLight = zone.PlayerLight.ToZonePlayerLightState();
        PlayerLightRadius = zone.PlayerLightRadius;
        PlayerLightStrength = zone.PlayerLightStrength;
        PlayerLightHue = zone.PlayerLightHue;
        PlayerLightSaturation = zone.PlayerLightSaturation;

        Fog = zone.Fog.ToZoneFogState();
        FogHue = zone.FogHue;
        FogSaturation = zone.FogSaturation;
        FogOpacity = zone.FogOpacity;
        FogDensity = zone.FogDensity;
        FogDirection = zone.FogDirection;
        FogSpeed = zone.FogSpeed;
        FogFeatherTop = zone.FogFeatherTop;
        FogFeatherRight = zone.FogFeatherRight;
        FogFeatherBottom = zone.FogFeatherBottom;
        FogFeatherLeft = zone.FogFeatherLeft;
        FogMarginTop = zone.FogMarginTop;
        FogMarginRight = zone.FogMarginRight;
        FogMarginBottom = zone.FogMarginBottom;
        FogMarginLeft = zone.FogMarginLeft;
        FogSmoothing = zone.FogSmoothing;

        Distortion = zone.Distortion.ToZoneDistortionState();
        DistortionStrength = zone.DistortionStrength;
        DistortionScale = zone.DistortionScale;
        DistortionSpeed = zone.DistortionSpeed;
        DistortionFeatherTop = zone.DistortionFeatherTop;
        DistortionFeatherRight = zone.DistortionFeatherRight;
        DistortionFeatherBottom = zone.DistortionFeatherBottom;
        DistortionFeatherLeft = zone.DistortionFeatherLeft;
        DistortionMarginTop = zone.DistortionMarginTop;
        DistortionMarginRight = zone.DistortionMarginRight;
        DistortionMarginBottom = zone.DistortionMarginBottom;
        DistortionMarginLeft = zone.DistortionMarginLeft;
        DistortionSmoothing = zone.DistortionSmoothing;
    }

    public ProtoZone AsProtoZone()
    {
        return new ProtoZone
        {
            Name = Name,
            Priority = Priority,
            Hue = Hue,
            Width = Width,
            Height = Height,
            MembershipRle = MembershipRle.Encode(Membership),

            Vision = Vision.ToProtoZoneVision(),
            VisionOutside = VisionOutside.ToProtoZoneVision(),
            HasVisionColor = HasVisionColor,
            VisionColor = VisionColor.ToInt(),

            CameraModeX = CameraModeX.ToProtoZoneCameraMode(),
            CameraModeY = CameraModeY.ToProtoZoneCameraMode(),
            CameraTarget = CameraTarget.ToProtoZoneCameraTarget(),
            CameraMovement = CameraMovement.ToProtoZoneCameraMovement(),
            CameraFollowMovement = CameraFollowMovement.ToProtoZoneCameraMovement(),

            Lighting = Lighting.ToProtoZoneLighting(),
            LightDarkness = LightDarkness,
            LightHue = LightHue,
            LightTint = LightTint,
            LightFeatherTop = LightFeatherTop,
            LightFeatherRight = LightFeatherRight,
            LightFeatherBottom = LightFeatherBottom,
            LightFeatherLeft = LightFeatherLeft,
            LightMarginTop = LightMarginTop,
            LightMarginRight = LightMarginRight,
            LightMarginBottom = LightMarginBottom,
            LightMarginLeft = LightMarginLeft,
            LightSmoothing = LightSmoothing,

            PlayerLight = PlayerLight.ToProtoZonePlayerLight(),
            PlayerLightRadius = PlayerLightRadius,
            PlayerLightStrength = PlayerLightStrength,
            PlayerLightHue = PlayerLightHue,
            PlayerLightSaturation = PlayerLightSaturation,

            Fog = Fog.ToProtoZoneFog(),
            FogHue = FogHue,
            FogSaturation = FogSaturation,
            FogOpacity = FogOpacity,
            FogDensity = FogDensity,
            FogDirection = FogDirection,
            FogSpeed = FogSpeed,
            FogFeatherTop = FogFeatherTop,
            FogFeatherRight = FogFeatherRight,
            FogFeatherBottom = FogFeatherBottom,
            FogFeatherLeft = FogFeatherLeft,
            FogMarginTop = FogMarginTop,
            FogMarginRight = FogMarginRight,
            FogMarginBottom = FogMarginBottom,
            FogMarginLeft = FogMarginLeft,
            FogSmoothing = FogSmoothing,

            Distortion = Distortion.ToProtoZoneDistortion(),
            DistortionStrength = DistortionStrength,
            DistortionScale = DistortionScale,
            DistortionSpeed = DistortionSpeed,
            DistortionFeatherTop = DistortionFeatherTop,
            DistortionFeatherRight = DistortionFeatherRight,
            DistortionFeatherBottom = DistortionFeatherBottom,
            DistortionFeatherLeft = DistortionFeatherLeft,
            DistortionMarginTop = DistortionMarginTop,
            DistortionMarginRight = DistortionMarginRight,
            DistortionMarginBottom = DistortionMarginBottom,
            DistortionMarginLeft = DistortionMarginLeft,
            DistortionSmoothing = DistortionSmoothing,
        };
    }

    /// <summary>
    /// Builds a create/update-settings request. The server ignores Width/Height/MembershipRle on
    /// this packet entirely — a newly created zone always starts with empty membership, and an
    /// existing zone's membership is untouched; membership can only be changed via area-edit
    /// requests (see <see cref="ZoneMembershipRects"/>).
    /// </summary>
    /// <param name="id">
    /// The id of an existing zone to update its settings on. Omit (or pass null/empty) to create
    /// a new zone instead — the server will assign it a fresh id.
    /// </param>
    public IMessage ToUpsertPacket(string? id = null)
    {
        var proto = AsProtoZone();
        if (!string.IsNullOrEmpty(id)) proto.Id = id;

        return new WorldZoneUpsertRequestPacket
        {
            Zone = proto,
        };
    }

    public bool IsBlockInZone(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return false;
        return Membership[x, y];
    }

    /// <summary>
    /// Compares every setting except <see cref="Membership"/> (and its Width/Height, which only
    /// describe the membership grid's dimensions). Used to decide whether an existing zone's
    /// settings need to be re-sent, independently of any membership diffing.
    /// </summary>
    public bool SettingsEqual(IZone other)
    {
        return Name == other.Name &&
               Priority == other.Priority &&
               Hue == other.Hue &&
               Vision == other.Vision &&
               VisionOutside == other.VisionOutside &&
               HasVisionColor == other.HasVisionColor &&
               VisionColor.EqualColor(other.VisionColor) &&
               CameraModeX == other.CameraModeX &&
               CameraModeY == other.CameraModeY &&
               CameraTarget == other.CameraTarget &&
               CameraMovement == other.CameraMovement &&
               CameraFollowMovement == other.CameraFollowMovement &&
               Lighting == other.Lighting &&
               LightDarkness == other.LightDarkness &&
               LightHue == other.LightHue &&
               LightTint == other.LightTint &&
               LightFeatherTop == other.LightFeatherTop &&
               LightFeatherRight == other.LightFeatherRight &&
               LightFeatherBottom == other.LightFeatherBottom &&
               LightFeatherLeft == other.LightFeatherLeft &&
               LightMarginTop == other.LightMarginTop &&
               LightMarginRight == other.LightMarginRight &&
               LightMarginBottom == other.LightMarginBottom &&
               LightMarginLeft == other.LightMarginLeft &&
               LightSmoothing == other.LightSmoothing &&
               PlayerLight == other.PlayerLight &&
               PlayerLightRadius == other.PlayerLightRadius &&
               PlayerLightStrength == other.PlayerLightStrength &&
               PlayerLightHue == other.PlayerLightHue &&
               PlayerLightSaturation == other.PlayerLightSaturation &&
               Fog == other.Fog &&
               FogHue == other.FogHue &&
               FogSaturation == other.FogSaturation &&
               FogOpacity == other.FogOpacity &&
               FogDensity == other.FogDensity &&
               FogDirection == other.FogDirection &&
               FogSpeed == other.FogSpeed &&
               FogFeatherTop == other.FogFeatherTop &&
               FogFeatherRight == other.FogFeatherRight &&
               FogFeatherBottom == other.FogFeatherBottom &&
               FogFeatherLeft == other.FogFeatherLeft &&
               FogMarginTop == other.FogMarginTop &&
               FogMarginRight == other.FogMarginRight &&
               FogMarginBottom == other.FogMarginBottom &&
               FogMarginLeft == other.FogMarginLeft &&
               FogSmoothing == other.FogSmoothing &&
               Distortion == other.Distortion &&
               DistortionStrength == other.DistortionStrength &&
               DistortionScale == other.DistortionScale &&
               DistortionSpeed == other.DistortionSpeed &&
               DistortionFeatherTop == other.DistortionFeatherTop &&
               DistortionFeatherRight == other.DistortionFeatherRight &&
               DistortionFeatherBottom == other.DistortionFeatherBottom &&
               DistortionFeatherLeft == other.DistortionFeatherLeft &&
               DistortionMarginTop == other.DistortionMarginTop &&
               DistortionMarginRight == other.DistortionMarginRight &&
               DistortionMarginBottom == other.DistortionMarginBottom &&
               DistortionMarginLeft == other.DistortionMarginLeft &&
               DistortionSmoothing == other.DistortionSmoothing;
    }

    protected bool Equals(Zone other)
    {
        return Name == other.Name &&
               Priority == other.Priority &&
               Hue == other.Hue &&
               Width == other.Width &&
               Height == other.Height &&
               MembershipEquals(other.Membership) &&
               Vision == other.Vision &&
               VisionOutside == other.VisionOutside &&
               HasVisionColor == other.HasVisionColor &&
               VisionColor.EqualColor(other.VisionColor) &&
               CameraModeX == other.CameraModeX &&
               CameraModeY == other.CameraModeY &&
               CameraTarget == other.CameraTarget &&
               CameraMovement == other.CameraMovement &&
               CameraFollowMovement == other.CameraFollowMovement &&
               Lighting == other.Lighting &&
               LightDarkness == other.LightDarkness &&
               LightHue == other.LightHue &&
               LightTint == other.LightTint &&
               LightFeatherTop == other.LightFeatherTop &&
               LightFeatherRight == other.LightFeatherRight &&
               LightFeatherBottom == other.LightFeatherBottom &&
               LightFeatherLeft == other.LightFeatherLeft &&
               LightMarginTop == other.LightMarginTop &&
               LightMarginRight == other.LightMarginRight &&
               LightMarginBottom == other.LightMarginBottom &&
               LightMarginLeft == other.LightMarginLeft &&
               LightSmoothing == other.LightSmoothing &&
               PlayerLight == other.PlayerLight &&
               PlayerLightRadius == other.PlayerLightRadius &&
               PlayerLightStrength == other.PlayerLightStrength &&
               PlayerLightHue == other.PlayerLightHue &&
               PlayerLightSaturation == other.PlayerLightSaturation &&
               Fog == other.Fog &&
               FogHue == other.FogHue &&
               FogSaturation == other.FogSaturation &&
               FogOpacity == other.FogOpacity &&
               FogDensity == other.FogDensity &&
               FogDirection == other.FogDirection &&
               FogSpeed == other.FogSpeed &&
               FogFeatherTop == other.FogFeatherTop &&
               FogFeatherRight == other.FogFeatherRight &&
               FogFeatherBottom == other.FogFeatherBottom &&
               FogFeatherLeft == other.FogFeatherLeft &&
               FogMarginTop == other.FogMarginTop &&
               FogMarginRight == other.FogMarginRight &&
               FogMarginBottom == other.FogMarginBottom &&
               FogMarginLeft == other.FogMarginLeft &&
               FogSmoothing == other.FogSmoothing &&
               Distortion == other.Distortion &&
               DistortionStrength == other.DistortionStrength &&
               DistortionScale == other.DistortionScale &&
               DistortionSpeed == other.DistortionSpeed &&
               DistortionFeatherTop == other.DistortionFeatherTop &&
               DistortionFeatherRight == other.DistortionFeatherRight &&
               DistortionFeatherBottom == other.DistortionFeatherBottom &&
               DistortionFeatherLeft == other.DistortionFeatherLeft &&
               DistortionMarginTop == other.DistortionMarginTop &&
               DistortionMarginRight == other.DistortionMarginRight &&
               DistortionMarginBottom == other.DistortionMarginBottom &&
               DistortionMarginLeft == other.DistortionMarginLeft &&
               DistortionSmoothing == other.DistortionSmoothing;
    }

    private bool MembershipEquals(bool[,] other)
    {
        if (Membership.GetLength(0) != other.GetLength(0) || Membership.GetLength(1) != other.GetLength(1))
            return false;

        for (var x = 0; x < Membership.GetLength(0); x++)
        {
            for (var y = 0; y < Membership.GetLength(1); y++)
            {
                if (Membership[x, y] != other[x, y]) return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Zone) obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name);
        hashCode.Add(Priority);
        hashCode.Add(Hue);
        hashCode.Add(Width);
        hashCode.Add(Height);
        hashCode.Add(Vision);
        hashCode.Add(VisionOutside);
        hashCode.Add(HasVisionColor);
        hashCode.Add(VisionColor);
        hashCode.Add(CameraModeX);
        hashCode.Add(CameraModeY);
        hashCode.Add(CameraTarget);
        hashCode.Add(CameraMovement);
        hashCode.Add(CameraFollowMovement);
        hashCode.Add(Lighting);
        hashCode.Add(LightDarkness);
        hashCode.Add(PlayerLight);
        hashCode.Add(Fog);
        hashCode.Add(Distortion);
        return hashCode.ToHashCode();
    }
}
