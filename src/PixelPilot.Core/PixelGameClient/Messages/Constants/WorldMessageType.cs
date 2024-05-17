﻿namespace PixelPilot.PixelGameClient.Messages.Constants;

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
    PlayerCheckpoint,
    PlayerRespawn,
    PlayerReset,
    PlayerCrown,
    PlayerKeyPressed,
    PlayerCounters,
    PlayerWin,
    PlayerLocalSwitchChanged,
    PlayerLocalSwitchReset,
    GlobalSwitchChanged,
    GlobalSwitchReset,
}