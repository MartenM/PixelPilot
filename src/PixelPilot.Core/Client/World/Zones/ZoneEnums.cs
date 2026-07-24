using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.World.Zones;

/// <summary>
/// Inherit takes the value from the next lower-priority zone, falling back to the global zone.
/// </summary>
public enum ZoneVisionState
{
    Inherit,
    Off,
    On
}

public static class ZoneVisionStateExtensions
{
    public static ZoneVisionState ToZoneVisionState(this ZoneVision vision)
    {
        switch (vision)
        {
            case ZoneVision.Inherit:
                return ZoneVisionState.Inherit;
            case ZoneVision.Off:
                return ZoneVisionState.Off;
            case ZoneVision.On:
                return ZoneVisionState.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(vision), vision, null);
        }
    }

    public static ZoneVision ToProtoZoneVision(this ZoneVisionState vision)
    {
        switch (vision)
        {
            case ZoneVisionState.Inherit:
                return ZoneVision.Inherit;
            case ZoneVisionState.Off:
                return ZoneVision.Off;
            case ZoneVisionState.On:
                return ZoneVision.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(vision), vision, null);
        }
    }
}

public enum ZoneCameraModeState
{
    Inherit,
    Follow,
    Lock
}

public static class ZoneCameraModeStateExtensions
{
    public static ZoneCameraModeState ToZoneCameraModeState(this ZoneCameraMode mode)
    {
        switch (mode)
        {
            case ZoneCameraMode.Inherit:
                return ZoneCameraModeState.Inherit;
            case ZoneCameraMode.Follow:
                return ZoneCameraModeState.Follow;
            case ZoneCameraMode.Lock:
                return ZoneCameraModeState.Lock;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    public static ZoneCameraMode ToProtoZoneCameraMode(this ZoneCameraModeState mode)
    {
        switch (mode)
        {
            case ZoneCameraModeState.Inherit:
                return ZoneCameraMode.Inherit;
            case ZoneCameraModeState.Follow:
                return ZoneCameraMode.Follow;
            case ZoneCameraModeState.Lock:
                return ZoneCameraMode.Lock;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }
}

public enum ZoneCameraTargetState
{
    Inherit,
    Whole,
    Part
}

public static class ZoneCameraTargetStateExtensions
{
    public static ZoneCameraTargetState ToZoneCameraTargetState(this ZoneCameraTarget target)
    {
        switch (target)
        {
            case ZoneCameraTarget.Inherit:
                return ZoneCameraTargetState.Inherit;
            case ZoneCameraTarget.Whole:
                return ZoneCameraTargetState.Whole;
            case ZoneCameraTarget.Part:
                return ZoneCameraTargetState.Part;
            default:
                throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
    }

    public static ZoneCameraTarget ToProtoZoneCameraTarget(this ZoneCameraTargetState target)
    {
        switch (target)
        {
            case ZoneCameraTargetState.Inherit:
                return ZoneCameraTarget.Inherit;
            case ZoneCameraTargetState.Whole:
                return ZoneCameraTarget.Whole;
            case ZoneCameraTargetState.Part:
                return ZoneCameraTarget.Part;
            default:
                throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
    }
}

public enum ZoneCameraMovementState
{
    Inherit,
    Smooth,
    Instant
}

public static class ZoneCameraMovementStateExtensions
{
    public static ZoneCameraMovementState ToZoneCameraMovementState(this ZoneCameraMovement movement)
    {
        switch (movement)
        {
            case ZoneCameraMovement.Inherit:
                return ZoneCameraMovementState.Inherit;
            case ZoneCameraMovement.Smooth:
                return ZoneCameraMovementState.Smooth;
            case ZoneCameraMovement.Instant:
                return ZoneCameraMovementState.Instant;
            default:
                throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
        }
    }

    public static ZoneCameraMovement ToProtoZoneCameraMovement(this ZoneCameraMovementState movement)
    {
        switch (movement)
        {
            case ZoneCameraMovementState.Inherit:
                return ZoneCameraMovement.Inherit;
            case ZoneCameraMovementState.Smooth:
                return ZoneCameraMovement.Smooth;
            case ZoneCameraMovementState.Instant:
                return ZoneCameraMovement.Instant;
            default:
                throw new ArgumentOutOfRangeException(nameof(movement), movement, null);
        }
    }
}

public enum ZoneLightingState
{
    Inherit,
    Off,
    On
}

public static class ZoneLightingStateExtensions
{
    public static ZoneLightingState ToZoneLightingState(this ZoneLighting lighting)
    {
        switch (lighting)
        {
            case ZoneLighting.Inherit:
                return ZoneLightingState.Inherit;
            case ZoneLighting.Off:
                return ZoneLightingState.Off;
            case ZoneLighting.On:
                return ZoneLightingState.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(lighting), lighting, null);
        }
    }

    public static ZoneLighting ToProtoZoneLighting(this ZoneLightingState lighting)
    {
        switch (lighting)
        {
            case ZoneLightingState.Inherit:
                return ZoneLighting.Inherit;
            case ZoneLightingState.Off:
                return ZoneLighting.Off;
            case ZoneLightingState.On:
                return ZoneLighting.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(lighting), lighting, null);
        }
    }
}

public enum ZoneFogState
{
    Inherit,
    Off,
    On
}

public static class ZoneFogStateExtensions
{
    public static ZoneFogState ToZoneFogState(this ZoneFog fog)
    {
        switch (fog)
        {
            case ZoneFog.Inherit:
                return ZoneFogState.Inherit;
            case ZoneFog.Off:
                return ZoneFogState.Off;
            case ZoneFog.On:
                return ZoneFogState.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(fog), fog, null);
        }
    }

    public static ZoneFog ToProtoZoneFog(this ZoneFogState fog)
    {
        switch (fog)
        {
            case ZoneFogState.Inherit:
                return ZoneFog.Inherit;
            case ZoneFogState.Off:
                return ZoneFog.Off;
            case ZoneFogState.On:
                return ZoneFog.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(fog), fog, null);
        }
    }
}

public enum ZoneDistortionState
{
    Inherit,
    Off,
    On
}

public static class ZoneDistortionStateExtensions
{
    public static ZoneDistortionState ToZoneDistortionState(this ZoneDistortion distortion)
    {
        switch (distortion)
        {
            case ZoneDistortion.Inherit:
                return ZoneDistortionState.Inherit;
            case ZoneDistortion.Off:
                return ZoneDistortionState.Off;
            case ZoneDistortion.On:
                return ZoneDistortionState.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(distortion), distortion, null);
        }
    }

    public static ZoneDistortion ToProtoZoneDistortion(this ZoneDistortionState distortion)
    {
        switch (distortion)
        {
            case ZoneDistortionState.Inherit:
                return ZoneDistortion.Inherit;
            case ZoneDistortionState.Off:
                return ZoneDistortion.Off;
            case ZoneDistortionState.On:
                return ZoneDistortion.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(distortion), distortion, null);
        }
    }
}

public enum ZonePlayerLightState
{
    Inherit,
    Off,
    On
}

public static class ZonePlayerLightStateExtensions
{
    public static ZonePlayerLightState ToZonePlayerLightState(this ZonePlayerLight playerLight)
    {
        switch (playerLight)
        {
            case ZonePlayerLight.Inherit:
                return ZonePlayerLightState.Inherit;
            case ZonePlayerLight.Off:
                return ZonePlayerLightState.Off;
            case ZonePlayerLight.On:
                return ZonePlayerLightState.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerLight), playerLight, null);
        }
    }

    public static ZonePlayerLight ToProtoZonePlayerLight(this ZonePlayerLightState playerLight)
    {
        switch (playerLight)
        {
            case ZonePlayerLightState.Inherit:
                return ZonePlayerLight.Inherit;
            case ZonePlayerLightState.Off:
                return ZonePlayerLight.Off;
            case ZonePlayerLightState.On:
                return ZonePlayerLight.On;
            default:
                throw new ArgumentOutOfRangeException(nameof(playerLight), playerLight, null);
        }
    }
}
