using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.World.Constants;

public enum PixelBlock : int
{
    Empty = 0,
    GravityLeft = 1,
    GravityUp = 2,
    GravityRight = 3,
    GravityDown = 4,
    GravityDot = 5,
    GravitySlowDot = 6,
    BoostLeft = 7,
    BoostUp = 8,
    BoostRight = 9,
    BoostDown = 10,
    Crown = 11,
    KeyRed = 20,
    KeyGreen = 21,
    KeyBlue = 22,
    Coin = 12,
    BlueCoin = 13,
    SpawnPoint = 14,
    Checkpoint = 15,
    Portal = 16,
    PortalInvisible = 17,
    Water = 18,
    WaterSurface = 19,
    KeyDoorRed = 23,
    KeyDoorGreen = 24,
    KeyDoorBlue = 25,
    CoinDoor = 26,
    BlueCoinDoor = 27,
    KeyGateRed = 28,
    KeyGateGreen = 29,
    KeyGateBlue = 30,
    CoinGate = 31,
    BlueCoinGate = 32,
    LocalSwitch = 38,
    LocalSwitchActivator = 39,
    LocalSwitchResetter = 40,
    LocalSwitchDoor = 41,
    LocalSwitchGate = 42,
    GlobalSwitch = 33,
    GlobalSwitchActivator = 34,
    GlobalSwitchResetter = 35,
    GlobalSwitchDoor = 36,
    GlobalSwitchGate = 37,
    DeathDoor = 43,
    DeathGate = 44,
    Spikes = 45,
    SpikesCenter = 46,
    Fire = 47,
    BasicGray = 48,
    BasicBlue = 49,
    BasicMagenta = 50,
    BasicRed = 51,
    BasicYellow = 52,
    BasicGreen = 53,
    BasicCyan = 54,
    BasicGrayBg = 55,
    BasicBlueBg = 56,
    BasicMagentaBg = 57,
    BasicRedBg = 58,
    BasicYellowBg = 59,
    BasicGreenBg = 60,
    BasicCyanBg = 61,
    BeveledMagenta = 62,
    BeveledGreen = 63,
    BeveledBlue = 64,
    BeveledRed = 65,
    BeveledYellow = 66,
    BeveledGray = 67,
    BricksBrown = 68,
    BricksTeal = 69,
    BricksPurple = 70,
    BricksGreen = 71,
    BricksRed = 72,
    BricksOlive = 73,
    BricksBrownBg = 74,
    BricksTealBg = 75,
    BricksPurpleBg = 76,
    BricksGreenBg = 77,
    BricksRedBg = 78,
    BricksOliveBg = 79,
    NormalGrayBg = 80,
    NormalBlueBg = 81,
    NormalMagentaBg = 82,
    NormalRedBg = 83,
    NormalYellowBg = 84,
    NormalGreenBg = 85,
    NormalCyanBg = 86,
    DarkGrayBg = 87,
    DarkBlueBg = 88,
    DarkMagentaBg = 89,
    DarkRedBg = 90,
    DarkYellowBg = 91,
    DarkGreenBg = 92,
    DarkCyanBg = 93,
    CheckerGrayBg = 94,
    CheckerBlueBg = 95,
    CheckerMagentaBg = 96,
    CheckerRedBg = 97,
    CheckerYellowBg = 98,
    CheckerGreenBg = 99,
    CheckerCyanBg = 100,
    PastelLightTanBg = 101,
    PastelLightGreenBg = 102,
    PastelReefBg = 103,
    PastelLightCyanBg = 104,
    PastelBlueBg = 105,
    PastelLightSalmonPinkBg = 106,
    BricksGrassLeftEdge = 108,
    BricksGrass = 107,
    BricksGrassRightEdge = 109,
    MetalSilver = 110,
    MetalCopper = 111,
    MetalGold = 112,
    HazardStripes = 113,
    FaceBlock = 114,
    BlackBlock = 115,
    FullBlackBlock = 116,
    SecretAppear = 117,
    SecretDisappear = 118,
    FactoryMetalCrate = 119,
    FactoryStone = 120,
    FactoryWood = 121,
    FactoryWoodenCrate = 122,
    FactoryScales = 123,
    GlassRed = 124,
    GlassOrange = 125,
    GlassYellow = 126,
    GlassGreen = 127,
    GlassCyan = 128,
    GlassBlue = 129,
    GlassPurple = 130,
    GlassMagenta = 131,
    MeadowGrassLeft = 132,
    MeadowGrassMiddle = 133,
    MeadowGrassRight = 134,
    MeadowBushLeft = 135,
    MeadowBushMiddle = 136,
    MeadowBushRight = 137,
    MeadowYellowFlower = 138,
    MeadowSmallBush = 139,
    EasterEggBlue = 140,
    EasterEggPink = 141,
    EasterEggYellow = 142,
    EasterEggRed = 143,
    EasterEggGreen = 144,
    CandyPink = 145,
    CandyBlue = 146,
    CandyPlatformPink = 147,
    CandyPlatformRed = 148,
    CandyPlatformCyan = 149,
    CandyPlatformGreen = 150,
    CandyCane = 151,
    CandyLicorice = 152,
    CandyChocolate = 153,
    CandyCreamSmall = 154,
    CandyCreamLarge = 155,
    CandyGumdropRed = 156,
    CandyGumdropGreen = 157,
    CandyGumdropPink = 158,
    CandyPinkBg = 159,
    CandyBlueBg = 160
}

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
}