using PixelPilot.Client.Messages.Constants;

namespace PixelPilot.Client.World.Constants;

public static class PixelBlockExtensions
{
    public static PacketFieldType[] GetPacketFieldTypes(this BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.Morphable:
                return new[] { PacketFieldType.Int32 };
            case BlockType.SwitchResetter:
                return new[] { PacketFieldType.Boolean };
            case BlockType.SwitchActivator:
                return new[] { PacketFieldType.Int32, PacketFieldType.Boolean };
            case BlockType.Portal:
                return new[] { PacketFieldType.String, PacketFieldType.String };
            case BlockType.WorldPortal:
                return new[] { PacketFieldType.String, PacketFieldType.Int32 };
            case BlockType.Sign:
                return new[] { PacketFieldType.String };
            case BlockType.EffectTimed:
            case BlockType.EffectLeveled:
                return new[] { PacketFieldType.Int32 };
            case BlockType.EffectTogglable:
                return new[] { PacketFieldType.Boolean };
            case BlockType.NoteBlock:
                return new[] { PacketFieldType.ByteArray };
            case BlockType.ColorBlock:
                return new[] { PacketFieldType.UInt32 };
            case BlockType.Normal:
                return Array.Empty<PacketFieldType>();
            default:
                throw new NotImplementedException("This specific block type is missing a packet implementation.");
        }
    }

    public static BlockType GetBlockType(this PixelBlock pixelBlock)
    {
        switch (pixelBlock)
        {
            case PixelBlock.CoinGoldDoor:
            case PixelBlock.CoinGoldGate:
            case PixelBlock.CoinBlueDoor:
            case PixelBlock.CoinBlueGate:
            case PixelBlock.HazardDeathDoor:
            case PixelBlock.HazardDeathGate:
            case PixelBlock.SwitchLocalToggle:
            case PixelBlock.SwitchLocalGate:
            case PixelBlock.SwitchLocalDoor:
            case PixelBlock.SwitchGlobalToggle:
            case PixelBlock.SwitchGlobalGate:
            case PixelBlock.SwitchGlobalDoor:
            case PixelBlock.ToolPortalWorldSpawn:
            case PixelBlock.CounterBlackDoor:
            case PixelBlock.CounterBlackReusableSet:
            case PixelBlock.CounterBlackGate:
            case PixelBlock.CounterBlackConsumable:
            case PixelBlock.CounterBlackConsumableSet:
            case PixelBlock.CounterBlackReusable:
            case PixelBlock.CounterWhiteDoor:
            case PixelBlock.CounterWhiteReusableSet:
            case PixelBlock.CounterWhiteGate:
            case PixelBlock.CounterWhiteConsumable:
            case PixelBlock.CounterWhiteConsumableSet:
            case PixelBlock.CounterWhiteReusable:
            case PixelBlock.CounterGrayDoor:
            case PixelBlock.CounterGrayReusableSet:
            case PixelBlock.CounterGrayGate:
            case PixelBlock.CounterGrayConsumable:
            case PixelBlock.CounterGrayConsumableSet:
            case PixelBlock.CounterGrayReusable:
            case PixelBlock.CounterRedDoor:
            case PixelBlock.CounterRedReusableSet:
            case PixelBlock.CounterRedGate:
            case PixelBlock.CounterRedConsumable:
            case PixelBlock.CounterRedConsumableSet:
            case PixelBlock.CounterRedReusable:
            case PixelBlock.CounterOrangeDoor:
            case PixelBlock.CounterOrangeReusableSet:
            case PixelBlock.CounterOrangeGate:
            case PixelBlock.CounterOrangeConsumable:
            case PixelBlock.CounterOrangeConsumableSet:
            case PixelBlock.CounterOrangeReusable:
            case PixelBlock.CounterYellowDoor:
            case PixelBlock.CounterYellowReusableSet:
            case PixelBlock.CounterYellowGate:
            case PixelBlock.CounterYellowConsumable:
            case PixelBlock.CounterYellowConsumableSet:
            case PixelBlock.CounterYellowReusable:
            case PixelBlock.CounterGreenDoor:
            case PixelBlock.CounterGreenReusableSet:
            case PixelBlock.CounterGreenGate:
            case PixelBlock.CounterGreenConsumable:
            case PixelBlock.CounterGreenConsumableSet:
            case PixelBlock.CounterGreenReusable:
            case PixelBlock.CounterCyanDoor:
            case PixelBlock.CounterCyanReusableSet:
            case PixelBlock.CounterCyanGate:
            case PixelBlock.CounterCyanConsumable:
            case PixelBlock.CounterCyanConsumableSet:
            case PixelBlock.CounterCyanReusable:
            case PixelBlock.CounterBlueDoor:
            case PixelBlock.CounterBlueReusableSet:
            case PixelBlock.CounterBlueGate:
            case PixelBlock.CounterBlueConsumable:
            case PixelBlock.CounterBlueConsumableSet:
            case PixelBlock.CounterBlueReusable:
            case PixelBlock.CounterMagentaGate:
            case PixelBlock.CounterMagentaDoor:
            case PixelBlock.CounterMagentaReusableSet:
            case PixelBlock.CounterMagentaConsumable:
            case PixelBlock.CounterMagentaConsumableSet:
            case PixelBlock.CounterMagentaReusable:
                return BlockType.Morphable;
            case PixelBlock.SwitchLocalResetter:
            case PixelBlock.SwitchGlobalResetter:
                return BlockType.SwitchResetter;
            case PixelBlock.SwitchLocalActivator:
            case PixelBlock.SwitchGlobalActivator:
                return BlockType.SwitchActivator;
            case PixelBlock.PortalVisibleDown:
            case PixelBlock.PortalVisibleUp:
            case PixelBlock.PortalVisibleLeft:
            case PixelBlock.PortalVisibleRight:
            case PixelBlock.PortalInvisibleDown:
            case PixelBlock.PortalInvisibleUp:
            case PixelBlock.PortalInvisibleLeft:
            case PixelBlock.PortalInvisibleRight:
                return BlockType.Portal;
            case PixelBlock.PortalWorld:
                return BlockType.WorldPortal;
            case PixelBlock.SignBlue:
            case PixelBlock.SignGold:
            case PixelBlock.SignGreen:
            case PixelBlock.SignNormal:
            case PixelBlock.SignRed:
            case PixelBlock.OuterspaceSignBlack:
            case PixelBlock.OuterspaceSignBlue:
            case PixelBlock.OuterspaceSignGreen:
            case PixelBlock.OuterspaceSignRed:
                return BlockType.Sign;
            case PixelBlock.EffectsJumpHeight:
                return BlockType.EffectLeveled;
            case PixelBlock.EffectsFly:
                return BlockType.EffectTogglable;
            case PixelBlock.EffectsSpeed:
                return BlockType.EffectLeveled;
            case PixelBlock.EffectsInvulnerability:
                return BlockType.EffectTogglable;
            case PixelBlock.EffectsCurse:
            case PixelBlock.EffectsPoison:
                return BlockType.EffectTimed;
            case PixelBlock.EffectsZombie:
                return BlockType.EffectTimed;
            case PixelBlock.EffectsMultiJump:
            case PixelBlock.EffectsGravityForce:
                return BlockType.EffectLeveled;
            case PixelBlock.NoteDrum:
            case PixelBlock.NoteGuitar:
            case PixelBlock.NotePiano:
                return BlockType.NoteBlock;
            case PixelBlock.CustomCheckerBg:
            case PixelBlock.CustomSolidBg:
                return BlockType.ColorBlock;
            default:
                return BlockType.Normal;
        }
    }
}