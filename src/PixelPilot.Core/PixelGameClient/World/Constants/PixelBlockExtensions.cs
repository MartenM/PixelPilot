using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.World.Constants;

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
            case PixelBlock.CoinDoor:
            case PixelBlock.CoinGate:
            case PixelBlock.BlueCoinDoor:
            case PixelBlock.BlueCoinGate:
            case PixelBlock.DeathDoor:
            case PixelBlock.DeathGate:
            case PixelBlock.Spikes:
            case PixelBlock.LocalSwitch:
            case PixelBlock.LocalSwitchDoor:
            case PixelBlock.LocalSwitchGate:
            case PixelBlock.GlobalSwitch:
            case PixelBlock.GlobalSwitchDoor:
            case PixelBlock.GlobalSwitchGate:
                return BlockType.Morphable;
            case PixelBlock.LocalSwitchResetter:
            case PixelBlock.GlobalSwitchResetter:
                return BlockType.SwitchResetter;
            case PixelBlock.LocalSwitchActivator:
            case PixelBlock.GlobalSwitchActivator:
                return BlockType.SwitchActivator;
            case PixelBlock.Portal:
            case PixelBlock.PortalInvisible:
                return BlockType.Portal;
            default:
                return BlockType.Normal;
        }
    }

    public static bool IsClimbable(this PixelBlock pixelBlock)
    {
        switch (pixelBlock)
        {
            case PixelBlock.JungleVineHorizontal:
            case PixelBlock.JungleVineVertical:
                return true;
            default:
                return false;
        }
    }
    
    public static bool IsBoost(this PixelBlock pixelBlock)
    {
        switch (pixelBlock)
        {
            case PixelBlock.BoostDown:
            case PixelBlock.BoostUp:
            case PixelBlock.BoostLeft:
            case PixelBlock.BoostRight:
                return true;
            default:
                return false;
        }
    }
    
    public static bool IsLiquide(this PixelBlock pixelBlock)
    {
        switch (pixelBlock)
        {
            case PixelBlock.Water:
                return true;
            default:
                return false;
        }
    }
}