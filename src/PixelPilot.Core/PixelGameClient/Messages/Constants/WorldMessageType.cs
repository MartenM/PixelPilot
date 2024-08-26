﻿namespace PixelPilot.PixelGameClient.Messages.Constants;

public enum WorldMessageType : int
{
    PlayerInit,
	PlayerUpdateRights,
	UpdateWorldMetadata,
	PerformWorldAction,
	WorldCleared,
	WorldReloaded,
	WorldBlockPlaced,
	WorldBlockFilled,
	PlayerChatMessage,
	OldChatMessages,
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
	PlayerTouchPlayer,
	PlayerAddEffect,
	PlayerRemoveEffect,
	PlayerResetEffects,
	PlayerTeam,
	PlayerCounters,
	PlayerLocalSwitchChanged,
	PlayerLocalSwitchReset,
	GlobalSwitchChanged,
	GlobalSwitchReset,
	PlayerDirectMessage,
}