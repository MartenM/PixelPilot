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
    public ZoneVisionState VisionCombine { get; set; }
    public ZoneVisionState VisionOutside { get; set; }
    public bool HasVisionColor { get; set; }
    public Color VisionColor { get; set; }

    public ZoneCameraModeState CameraModeX { get; set; }
    public ZoneCameraModeState CameraModeY { get; set; }
    public ZoneCameraTargetState CameraTarget { get; set; }
    public ZoneCameraMovementState CameraMovement { get; set; }
    public ZoneCameraMovementState CameraFollowMovement { get; set; }

    public ZoneLightingState Lighting { get; set; }
    public Color LightColor { get; set; } = Color.White;
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
    public Color PlayerLightColor { get; set; } = Color.White;

    public ZoneFogState Fog { get; set; }
    public Color FogColor { get; set; } = Color.White;
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
        VisionCombine = zone.VisionCombine;
        VisionOutside = zone.VisionOutside;
        HasVisionColor = zone.HasVisionColor;
        VisionColor = zone.VisionColor;

        CameraModeX = zone.CameraModeX;
        CameraModeY = zone.CameraModeY;
        CameraTarget = zone.CameraTarget;
        CameraMovement = zone.CameraMovement;
        CameraFollowMovement = zone.CameraFollowMovement;

        Lighting = zone.Lighting;
        LightColor = zone.LightColor;
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
        PlayerLightColor = zone.PlayerLightColor;

        Fog = zone.Fog;
        FogColor = zone.FogColor;
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

    /// <param name="protoZone">The zone as received from the server.</param>
    /// <param name="worldWidth">Width of the world the zone belongs to.</param>
    /// <param name="worldHeight">Height of the world the zone belongs to.</param>
    public static Zone FromProtoZone(ProtoZone protoZone, int worldWidth, int worldHeight)
    {
        var zone = new Zone();
        zone.UpdateWithProtoZone(protoZone, worldWidth, worldHeight);
        return zone;
    }

    /// <summary>
    /// Overwrites this zone with the server's state. The membership mask on the wire always spans
    /// the whole world, so the world's dimensions are needed to decode it.
    /// </summary>
    public void UpdateWithProtoZone(ProtoZone zone, int worldWidth, int worldHeight)
    {
        Name = zone.Name;
        Priority = zone.Priority;
        Hue = zone.Hue;
        Width = worldWidth;
        Height = worldHeight;
        Membership = MembershipRle.Decode(zone.MembershipRle, worldWidth, worldHeight);

        Vision = zone.Vision.ToZoneVisionState();
        VisionCombine = zone.VisionCombine.ToZoneVisionState();
        VisionOutside = zone.VisionOutside.ToZoneVisionState();
        HasVisionColor = zone.HasVisionColor;
        VisionColor = FromRgb(zone.VisionColor);

        CameraModeX = zone.CameraModeX.ToZoneCameraModeState();
        CameraModeY = zone.CameraModeY.ToZoneCameraModeState();
        CameraTarget = zone.CameraTarget.ToZoneCameraTargetState();
        CameraMovement = zone.CameraMovement.ToZoneCameraMovementState();
        CameraFollowMovement = zone.CameraFollowMovement.ToZoneCameraMovementState();

        Lighting = zone.Lighting.ToZoneLightingState();
        LightColor = FromRgb(zone.LightColor);
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
        PlayerLightColor = FromRgb(zone.PlayerLightColor);

        Fog = zone.Fog.ToZoneFogState();
        FogColor = FromRgb(zone.FogColor);
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
            MembershipRle = MembershipRle.Encode(Membership),

            Vision = Vision.ToProtoZoneVision(),
            VisionCombine = VisionCombine.ToProtoZoneVision(),
            VisionOutside = VisionOutside.ToProtoZoneVision(),
            HasVisionColor = HasVisionColor,
            VisionColor = ToRgb(VisionColor),

            CameraModeX = CameraModeX.ToProtoZoneCameraMode(),
            CameraModeY = CameraModeY.ToProtoZoneCameraMode(),
            CameraTarget = CameraTarget.ToProtoZoneCameraTarget(),
            CameraMovement = CameraMovement.ToProtoZoneCameraMovement(),
            CameraFollowMovement = CameraFollowMovement.ToProtoZoneCameraMovement(),

            Lighting = Lighting.ToProtoZoneLighting(),
            LightColor = ToRgb(LightColor),
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
            PlayerLightColor = ToRgb(PlayerLightColor),

            Fog = Fog.ToProtoZoneFog(),
            FogColor = ToRgb(FogColor),
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
    /// Builds a create/update-settings request. The server ignores MembershipRle on this packet
    /// entirely — a newly created zone always starts with empty membership, and an
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

    /// <summary>
    /// Zone colors are packed 0xRRGGBB on the wire, without alpha.
    /// </summary>
    private static Color FromRgb(int rgb) => Color.FromArgb(255, (rgb >> 16) & 0xFF, (rgb >> 8) & 0xFF, rgb & 0xFF);

    private static int ToRgb(Color color) => color.R << 16 | color.G << 8 | color.B;

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
               VisionCombine == other.VisionCombine &&
               VisionOutside == other.VisionOutside &&
               HasVisionColor == other.HasVisionColor &&
               VisionColor.EqualColor(other.VisionColor) &&
               CameraModeX == other.CameraModeX &&
               CameraModeY == other.CameraModeY &&
               CameraTarget == other.CameraTarget &&
               CameraMovement == other.CameraMovement &&
               CameraFollowMovement == other.CameraFollowMovement &&
               Lighting == other.Lighting &&
               LightColor.EqualColor(other.LightColor) &&
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
               PlayerLightColor.EqualColor(other.PlayerLightColor) &&
               Fog == other.Fog &&
               FogColor.EqualColor(other.FogColor) &&
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
               VisionCombine == other.VisionCombine &&
               VisionOutside == other.VisionOutside &&
               HasVisionColor == other.HasVisionColor &&
               VisionColor.EqualColor(other.VisionColor) &&
               CameraModeX == other.CameraModeX &&
               CameraModeY == other.CameraModeY &&
               CameraTarget == other.CameraTarget &&
               CameraMovement == other.CameraMovement &&
               CameraFollowMovement == other.CameraFollowMovement &&
               Lighting == other.Lighting &&
               LightColor.EqualColor(other.LightColor) &&
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
               PlayerLightColor.EqualColor(other.PlayerLightColor) &&
               Fog == other.Fog &&
               FogColor.EqualColor(other.FogColor) &&
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
        hashCode.Add(VisionCombine);
        hashCode.Add(VisionOutside);
        hashCode.Add(HasVisionColor);
        hashCode.Add(VisionColor);
        hashCode.Add(CameraModeX);
        hashCode.Add(CameraModeY);
        hashCode.Add(CameraTarget);
        hashCode.Add(CameraMovement);
        hashCode.Add(CameraFollowMovement);
        hashCode.Add(Lighting);
        hashCode.Add(LightColor);
        hashCode.Add(PlayerLight);
        hashCode.Add(Fog);
        hashCode.Add(Distortion);
        return hashCode.ToHashCode();
    }
}
