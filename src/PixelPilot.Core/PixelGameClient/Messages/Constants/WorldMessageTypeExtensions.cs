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
            WorldMessageType.PlayerChatMessage => typeof(PlayerChatPacket),
            WorldMessageType.PlayerMoved => typeof(PlayerMovePacket),
            WorldMessageType.PlayerGodMode => typeof(PlayerGodmodePacket),
            WorldMessageType.PlayerModMode => typeof(PlayerModMode),
            WorldMessageType.PlayerFace => typeof(PlayerFacePacket),
            WorldMessageType.PlayerCounters => typeof(PlayerStatsChangePacket),
            WorldMessageType.PlayerRespawn => typeof(PlayerRespawnPacket),
            WorldMessageType.WorldBlockPlaced => typeof(WorldBlockPlacedPacket),
            WorldMessageType.PlayerUpdateRights => typeof(PlayerUpdateRightsPacket),
            WorldMessageType.PlayerReset => typeof(PlayerResetPacket),
            WorldMessageType.PlayerTeleported => typeof(PlayerTeleportedPacket),
            WorldMessageType.PlayerTouchBlock => typeof(PlayerTouchBlockPacket),
            WorldMessageType.PlayerDirectMessage => typeof(PlayerPrivateMessagePacket),
            WorldMessageType.PlayerLocalSwitchChanged => typeof(LocalSwitchChangedPacket),
            WorldMessageType.PlayerLocalSwitchReset => typeof(LocalSwitchResetPacket),
            WorldMessageType.GlobalSwitchChanged => typeof(GlobalSwitchChangedPacket),
            WorldMessageType.GlobalSwitchReset => typeof(GlobalSwitchResetPacket),
            WorldMessageType.WorldReloaded => typeof(WorldReloadedPacket),
            WorldMessageType.WorldCleared => typeof(WorldClearedPacket),
            WorldMessageType.UpdateWorldMetadata => typeof(WorldMetaPacket),
            WorldMessageType.SystemMessage => typeof(SystemMessagePacket),
            WorldMessageType.OldChatMessages => typeof(OldChatMessagesPacket),
            WorldMessageType.PlayerAddEffect => typeof(PlayerAddEffectPacket),
            WorldMessageType.PlayerTeam => typeof(PlayerTeamPacket),
            WorldMessageType.PlayerTouchPlayer => typeof(PlayerTouchPlayerPacket),
            WorldMessageType.PlayerResetEffects => typeof(PlayerResetEffectsPacket),
            WorldMessageType.PlayerRemoveEffect => typeof(PlayerRemoveEffectPacket),
            _ => null
        };
    }
}