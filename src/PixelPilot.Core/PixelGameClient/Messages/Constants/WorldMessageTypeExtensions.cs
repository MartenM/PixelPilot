using PixelPilot.PixelGameClient.Messages.Received;

namespace PixelPilot.PixelGameClient.Messages.Constants;

static class WorldMessageTypeExtensions
{
    public static Type? AsPacketType(this WorldMessageType type)
    {
        return type switch
        {
            WorldMessageType.PlayerInit => typeof(InitPacket),
            WorldMessageType.PlayerJoined => typeof(PlayerJoinPacket),
            WorldMessageType.PlayerLeft => typeof(PlayerLeftPacket),
            WorldMessageType.ChatMessage => typeof(PlayerChatPacket),
            WorldMessageType.PlayerMoved => typeof(PlayerMovePacket),
            WorldMessageType.PlayerGodMode => typeof(PlayerGodmodePacket),
            WorldMessageType.PlayerModMode => typeof(PlayerModMode),
            WorldMessageType.PlayerFace => typeof(PlayerFacePacket),
            WorldMessageType.PlayerCounters => typeof(PlayerStatsChangePacket),
            WorldMessageType.PlayerRespawn => typeof(PlayerRespawnPacket),
            WorldMessageType.WorldBlockPlaced => typeof(WorldBlockPlacedPacket),
            WorldMessageType.PlayerCrown => typeof(PlayerCrownPacket),
            WorldMessageType.UpdateRights => typeof(PlayerUpdateRightsPacket),
            WorldMessageType.PlayerReset => typeof(PlayerResetPacket),
            WorldMessageType.PlayerTeleported => typeof(PlayerTeleportedPacket),
            WorldMessageType.PlayerCheckpoint => typeof(CheckpointPacket),
            WorldMessageType.PlayerKeyPressed => typeof(KeyPressedPacket),
            WorldMessageType.PlayerLocalSwitchChanged => typeof(LocalSwitchChangedPacket),
            WorldMessageType.PlayerLocalSwitchReset => typeof(LocalSwitchResetPacket),
            WorldMessageType.GlobalSwitchChanged => typeof(GlobalSwitchChangedPacket),
            WorldMessageType.GlobalSwitchReset => typeof(GlobalSwitchResetPacket),
            WorldMessageType.WorldReloaded => typeof(WorldReloadedPacket),
            WorldMessageType.WorldCleared => typeof(WorldClearedPacket),
            WorldMessageType.WorldMetadata => typeof(WorldMetaPacket),
            WorldMessageType.SystemMessage => typeof(SystemMessagePacket),
            _ => null
        };
    }
}