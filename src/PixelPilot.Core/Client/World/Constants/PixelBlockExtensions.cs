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
                return new[] { PacketFieldType.Int32, PacketFieldType.Int32, PacketFieldType.Int32 };
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
                return BlockType.Morphable;
            case PixelBlock.SwitchLocalResetter:
            case PixelBlock.SwitchGlobalResetter:
                return BlockType.SwitchResetter;
            case PixelBlock.SwitchLocalActivator:
            case PixelBlock.SwitchGlobalActivator:
                return BlockType.SwitchActivator;
            case PixelBlock.Portal:
            case PixelBlock.PortalInvisible:
                return BlockType.Portal;
            case PixelBlock.PortalWorld:
                return BlockType.WorldPortal;
            case PixelBlock.SignBlue:
            case PixelBlock.SignGold:
            case PixelBlock.SignGreen:
            case PixelBlock.SignNormal:
            case PixelBlock.SignRed:
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
                return BlockType.EffectTimed;
            case PixelBlock.EffectsZombie:
                return BlockType.EffectTimed;
            case PixelBlock.EffectsGravityforce:
                return BlockType.EffectLeveled;
            case PixelBlock.EffectsMultiJump:
                return BlockType.EffectLeveled;
            case PixelBlock.NoteDrum:
            case PixelBlock.NoteGuitar:
            case PixelBlock.NotePiano:
                return BlockType.NoteBlock;
            default:
                return BlockType.Normal;
        }
    }
}