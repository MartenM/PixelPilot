﻿using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerRemoveEffectPacket : IPixelGamePlayerPacket, IPacketOutConvertible
{
    public PlayerRemoveEffectPacket(int id, int effectId)
    {
        PlayerId = id;
        EffectId = effectId;
    }

    public int PlayerId { get; }
    public int EffectId { get; }
    public IPixelGamePacketOut AsPacketOut()
    {
        return new PlayerEffectRemovedOutPacket(EffectId);
    }
}