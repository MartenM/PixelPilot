namespace PixelPilot.PixelGameClient.Messages.Constants;

public enum WorldMessageType : int
{
    PlayerInit,
    UpdateRights,
    WorldMetadata,
    WorldCleared,
    WorldReloaded,
    WorldBlockPlaced,
    ChatMessage,
    SystemMessage,
    PlayerJoined,
    PlayerLeft,
    PlayerMoved,
    PlayerTeleported,
    PlayerFace,
    PlayerGodMode,
    PlayerModMode,
    PlayerRespawn,
    PlayerReset,
    PlayerTouchBlock,
    PlayerCounters,
    PlayerLocalSwitchChanged,
    PlayerLocalSwitchReset,
    GlobalSwitchChanged,
    GlobalSwitchReset,
    PlayerPrivateMessage,
}