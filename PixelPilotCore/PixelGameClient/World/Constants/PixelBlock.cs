using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.World.Constants;

/// <summary>
/// All blocks available in the game as of 29-04-2024.
/// </summary>
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
    LocalSwitch = 33,
    LocalSwitchActivator = 34,
    LocalSwitchResetter = 35,
    LocalSwitchDoor = 36,
    LocalSwitchGate = 37,
    GlobalSwitch = 38,
    GlobalSwitchActivator = 39,
    GlobalSwitchResetter = 40,
    GlobalSwitchDoor = 41,
    GlobalSwitchGate = 42,
    DeathDoor = 43,
    DeathGate = 44,
    Spikes = 45,
    SpikesCenter = 46,
    Fire = 47,
    BasicWhite = 48,
    BasicGray = 49,
    BasicBlack = 50,
    BasicRed = 51,
    BasicOrange = 52,
    BasicYellow = 53,
    BasicGreen = 54,
    BasicCyan = 55,
    BasicBlue = 56,
    BasicMagenta = 57,
    BasicWhiteBg = 58,
    BasicGrayBg = 59,
    BasicBlackBg = 60,
    BasicRedBg = 61,
    BasicOrangeBg = 62,
    BasicYellowBg = 63,
    BasicGreenBg = 64,
    BasicCyanBg = 65,
    BasicBlueBg = 66,
    BasicMagentaBg = 67,
    BeveledWhite = 68,
    BeveledGray = 69,
    BeveledBlack = 70,
    BeveledRed = 71,
    BeveledOrange = 72,
    BeveledYellow = 73,
    BeveledGreen = 74,
    BeveledCyan = 75,
    BeveledBlue = 76,
    BeveledMagenta = 77,
    BricksWhite = 78,
    BricksGray = 79,
    BricksBlack = 80,
    BricksRed = 81,
    BricksBrown = 82,
    BricksOlive = 83,
    BricksGreen = 84,
    BricksTeal = 85,
    BricksBlue = 86,
    BricksPurple = 87,
    BricksWhiteBg = 88,
    BricksGrayBg = 89,
    BricksBlackBg = 90,
    BricksRedBg = 91,
    BricksBrownBg = 92,
    BricksOliveBg = 93,
    BricksGreenBg = 94,
    BricksTealBg = 95,
    BricksBlueBg = 96,
    BricksPurpleBg = 97,
    NormalWhiteBg = 98,
    NormalGrayBg = 99,
    NormalBlackBg = 100,
    NormalRedBg = 101,
    NormalOrangeBg = 102,
    NormalYellowBg = 103,
    NormalGreenBg = 104,
    NormalCyanBg = 105,
    NormalBlueBg = 106,
    NormalMagentaBg = 107,
    DarkWhiteBg = 108,
    DarkGrayBg = 109,
    DarkBlackBg = 110,
    DarkRedBg = 111,
    DarkOrangeBg = 112,
    DarkYellowBg = 113,
    DarkGreenBg = 114,
    DarkCyanBg = 115,
    DarkBlueBg = 116,
    DarkMagentaBg = 117,
    CheckerWhiteBg = 118,
    CheckerGrayBg = 119,
    CheckerBlackBg = 120,
    CheckerRedBg = 121,
    CheckerOrangeBg = 122,
    CheckerYellowBg = 123,
    CheckerGreenBg = 124,
    CheckerCyanBg = 125,
    CheckerBlueBg = 126,
    CheckerMagentaBg = 127,
    PastelRedBg = 128,
    PastelOrangeBg = 129,
    PastelYellowBg = 130,
    PastelLimeBg = 131,
    PastelGreenBg = 132,
    PastelCyanBg = 133,
    PastelBlueBg = 134,
    PastelPurpleBg = 135,
    BricksGrassLeftEdge = 137,
    BricksGrass = 136,
    BricksGrassRightEdge = 138,
    MetalSilver = 139,
    MetalCopper = 140,
    MetalGold = 141,
    HazardStripes = 142,
    DarkHazardStripes = 143,
    FaceBlock = 144,
    NoFaceBlock = 145,
    BlackBlock = 146,
    FullBlackBlock = 147,
    SecretAppear = 148,
    SecretDisappear = 149,
    SecretInvisible = 150,
    GlassRed = 151,
    GlassOrange = 152,
    GlassYellow = 153,
    GlassGreen = 154,
    GlassCyan = 155,
    GlassBlue = 156,
    GlassPurple = 157,
    GlassMagenta = 158,
    MineralsRed = 159,
    MineralsOrange = 160,
    MineralsYellow = 161,
    MineralsGreen = 162,
    MineralsCyan = 163,
    MineralsBlue = 164,
    MineralsPurple = 165,
    MineralsMagenta = 166,
    FactoryMetalCrate = 167,
    FactoryStone = 168,
    FactoryWood = 169,
    FactoryWoodenCrate = 170,
    FactoryScales = 171,
    MeadowGrassLeft = 172,
    MeadowGrassMiddle = 173,
    MeadowGrassRight = 174,
    MeadowBushLeft = 175,
    MeadowBushMiddle = 176,
    MeadowBushRight = 177,
    MeadowYellowFlower = 178,
    MeadowSmallBush = 179,
    EasterEggBlue = 180,
    EasterEggPink = 181,
    EasterEggYellow = 182,
    EasterEggRed = 183,
    EasterEggGreen = 184,
    CandyPink = 185,
    CandyBlue = 186,
    CandyPlatformPink = 187,
    CandyPlatformRed = 188,
    CandyPlatformCyan = 189,
    CandyPlatformGreen = 190,
    CandyCane = 191,
    CandyLicorice = 192,
    CandyChocolate = 193,
    CandyCreamSmall = 194,
    CandyCreamLarge = 195,
    CandyGumdropRed = 196,
    CandyGumdropGreen = 197,
    CandyGumdropPink = 198,
    CandyPinkBg = 199,
    CandyBlueBg = 200,
    BeachSand = 201,
    BeachParasol = 202,
    BeachSandPileRight = 203,
    BeachSandPileLeft = 204,
    BeachRock = 205,
    BeachDryBush = 206,
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