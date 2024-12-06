namespace PixelPilot.Client.World.Constants;

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
    InvisibleGravityLeft = 7,
    InvisibleGravityUp = 8,
    InvisibleGravityRight = 9,
    InvisibleGravityDown = 10,
    InvisibleGravityDot = 11,
    InvisibleGravitySlowDot = 12,
    BoostLeft = 13,
    BoostUp = 14,
    BoostRight = 15,
    BoostDown = 16,
    ClimbableVineVertical = 17,
    ClimbableVineHorizontal = 18,
    ClimbableChainLightVertical = 19,
    ClimbableChainLightHorizontal = 20,
    ClimbableChainDarkVertical = 21,
    ClimbableChainDarkHorizontal = 22,
    ClimbableLadderMetal = 23,
    ClimbableLadderWood = 24,
    ClimbableRopeVertical = 25,
    ClimbableRopeHorizontal = 26,
    ClimbableLadderStalkSmall = 27,
    ClimbableLadderStalkLarge = 28,
    ClimbableLatticeVine = 29,
    SpecialHologram = 30,
    SpecialDiamond = 31,
    SpecialCake = 32,
    CrownGold = 33,
    CrownSilver = 34,
    CrownGoldDoor = 35,
    CrownGoldGate = 36,
    CrownSilverDoor = 37,
    CrownSilverGate = 38,
    KeyRed = 83,
    KeyGreen = 84,
    KeyBlue = 85,
    KeyCyan = 86,
    KeyMagenta = 87,
    KeyYellow = 88,
    KeyRedDoor = 89,
    KeyGreenDoor = 90,
    KeyBlueDoor = 91,
    KeyCyanDoor = 92,
    KeyMagentaDoor = 93,
    KeyYellowDoor = 94,
    KeyRedGate = 95,
    KeyGreenGate = 96,
    KeyBlueGate = 97,
    KeyCyanGate = 98,
    KeyMagentaGate = 99,
    KeyYellowGate = 100,
    CoinGold = 39,
    CoinBlue = 40,
    CoinGoldDoor = 41,
    CoinBlueDoor = 42,
    CoinGoldGate = 43,
    CoinBlueGate = 44,
    EffectsJumpHeight = 45,
    EffectsFly = 46,
    EffectsSpeed = 47,
    EffectsInvulnerability = 48,
    EffectsCurse = 49,
    EffectsZombie = 50,
    EffectsGravityforce = 51,
    EffectsMultiJump = 52,
    EffectsGravityLeft = 53,
    EffectsGravityUp = 54,
    EffectsGravityRight = 55,
    EffectsGravityDown = 56,
    EffectsGravityOff = 57,
    EffectsOff = 58,
    EffectsZombieDoor = 59,
    EffectsZombieGate = 60,
    ToolSpawnLobby = 61,
    ToolCheckpoint = 62,
    ToolReset = 63,
    ToolGodModeActivator = 64,
    ToolPortalWorldSpawn = 65,
    ToolActivateMinimap = 66,
    SignNormal = 67,
    SignRed = 68,
    SignGreen = 69,
    SignBlue = 70,
    SignGold = 71,
    Portal = 72,
    PortalInvisible = 73,
    PortalWorld = 74,
    LiquidWater = 75,
    LiquidWaterSurface = 76,
    LiquidWaste = 77,
    LiquidWasteSurface = 78,
    LiquidLava = 79,
    LiquidLavaSurface = 80,
    LiquidMud = 81,
    LiquidMudSurface = 82,
    SwitchLocalToggle = 101,
    SwitchLocalActivator = 102,
    SwitchLocalResetter = 103,
    SwitchLocalDoor = 104,
    SwitchLocalGate = 105,
    SwitchGlobalToggle = 106,
    SwitchGlobalActivator = 107,
    SwitchGlobalResetter = 108,
    SwitchGlobalDoor = 109,
    SwitchGlobalGate = 110,
    HazardSpikesBrownLeft = 114,
    HazardSpikesBrownUp = 111,
    HazardSpikesBrownRight = 112,
    HazardSpikesBrownDown = 113,
    HazardSpikesBrownCenter = 115,
    HazardSpikesWhiteLeft = 119,
    HazardSpikesWhiteUp = 116,
    HazardSpikesWhiteRight = 117,
    HazardSpikesWhiteDown = 118,
    HazardSpikesWhiteCenter = 120,
    HazardSpikesGrayLeft = 124,
    HazardSpikesGrayUp = 121,
    HazardSpikesGrayRight = 122,
    HazardSpikesGrayDown = 123,
    HazardSpikesGrayCenter = 125,
    HazardSpikesRedLeft = 129,
    HazardSpikesRedUp = 126,
    HazardSpikesRedRight = 127,
    HazardSpikesRedDown = 128,
    HazardSpikesRedCenter = 130,
    HazardSpikesYellowLeft = 134,
    HazardSpikesYellowUp = 131,
    HazardSpikesYellowRight = 132,
    HazardSpikesYellowDown = 133,
    HazardSpikesYellowCenter = 135,
    HazardSpikesGreenLeft = 139,
    HazardSpikesGreenUp = 136,
    HazardSpikesGreenRight = 137,
    HazardSpikesGreenDown = 138,
    HazardSpikesGreenCenter = 140,
    HazardSpikesBlueLeft = 144,
    HazardSpikesBlueUp = 141,
    HazardSpikesBlueRight = 142,
    HazardSpikesBlueDown = 143,
    HazardSpikesBlueCenter = 145,
    HazardFire = 146,
    HazardDeathDoor = 147,
    HazardDeathGate = 148,
    NoteDrum = 149,
    NotePiano = 150,
    NoteGuitar = 151,
    TeamEffectNone = 152,
    TeamEffectRed = 153,
    TeamEffectGreen = 154,
    TeamEffectBlue = 155,
    TeamEffectCyan = 156,
    TeamEffectMagenta = 157,
    TeamEffectYellow = 158,
    TeamNoneDoor = 159,
    TeamRedDoor = 160,
    TeamGreenDoor = 161,
    TeamBlueDoor = 162,
    TeamCyanDoor = 163,
    TeamMagentaDoor = 164,
    TeamYellowDoor = 165,
    TeamNoneGate = 166,
    TeamRedGate = 167,
    TeamGreenGate = 168,
    TeamBlueGate = 169,
    TeamCyanGate = 170,
    TeamMagentaGate = 171,
    TeamYellowGate = 172,
    BasicWhite = 173,
    BasicGray = 174,
    BasicBlack = 175,
    BasicRed = 176,
    BasicOrange = 177,
    BasicYellow = 178,
    BasicGreen = 179,
    BasicCyan = 180,
    BasicBlue = 181,
    BasicMagenta = 182,
    BasicWhiteBg = 183,
    BasicGrayBg = 184,
    BasicBlackBg = 185,
    BasicRedBg = 186,
    BasicOrangeBg = 187,
    BasicYellowBg = 188,
    BasicGreenBg = 189,
    BasicCyanBg = 190,
    BasicBlueBg = 191,
    BasicMagentaBg = 192,
    BeveledWhite = 193,
    BeveledGray = 194,
    BeveledBlack = 195,
    BeveledRed = 196,
    BeveledOrange = 197,
    BeveledYellow = 198,
    BeveledGreen = 199,
    BeveledCyan = 200,
    BeveledBlue = 201,
    BeveledMagenta = 202,
    BeveledWhiteBg = 203,
    BeveledGrayBg = 204,
    BeveledBlackBg = 205,
    BeveledRedBg = 206,
    BeveledOrangeBg = 207,
    BeveledYellowBg = 208,
    BeveledGreenBg = 209,
    BeveledCyanBg = 210,
    BeveledBlueBg = 211,
    BeveledMagentaBg = 212,
    BrickWhite = 213,
    BrickGray = 214,
    BrickBlack = 215,
    BrickRed = 216,
    BrickBrown = 217,
    BrickOlive = 218,
    BrickGreen = 219,
    BrickTeal = 220,
    BrickBlue = 221,
    BrickPurple = 222,
    BrickWhiteBg = 223,
    BrickGrayBg = 224,
    BrickBlackBg = 225,
    BrickRedBg = 226,
    BrickBrownBg = 227,
    BrickOliveBg = 228,
    BrickGreenBg = 229,
    BrickTealBg = 230,
    BrickBlueBg = 231,
    BrickPurpleBg = 232,
    NormalWhiteBg = 233,
    NormalGrayBg = 234,
    NormalBlackBg = 235,
    NormalRedBg = 236,
    NormalOrangeBg = 237,
    NormalYellowBg = 238,
    NormalGreenBg = 239,
    NormalCyanBg = 240,
    NormalBlueBg = 241,
    NormalMagentaBg = 242,
    DarkWhiteBg = 243,
    DarkGrayBg = 244,
    DarkBlackBg = 245,
    DarkRedBg = 246,
    DarkOrangeBg = 247,
    DarkYellowBg = 248,
    DarkGreenBg = 249,
    DarkCyanBg = 250,
    DarkBlueBg = 251,
    DarkMagentaBg = 252,
    CheckerWhite = 253,
    CheckerGray = 254,
    CheckerBlack = 255,
    CheckerRed = 256,
    CheckerOrange = 257,
    CheckerYellow = 258,
    CheckerGreen = 259,
    CheckerCyan = 260,
    CheckerBlue = 261,
    CheckerMagenta = 262,
    CheckerWhiteBg = 263,
    CheckerGrayBg = 264,
    CheckerBlackBg = 265,
    CheckerRedBg = 266,
    CheckerOrangeBg = 267,
    CheckerYellowBg = 268,
    CheckerGreenBg = 269,
    CheckerCyanBg = 270,
    CheckerBlueBg = 271,
    CheckerMagentaBg = 272,
    PastelRedBg = 273,
    PastelOrangeBg = 274,
    PastelYellowBg = 275,
    PastelLimeBg = 276,
    PastelGreenBg = 277,
    PastelCyanBg = 278,
    PastelBlueBg = 279,
    PastelPurpleBg = 280,
    GrassBrickLeft = 282,
    GrassBrickMiddle = 281,
    GrassBrickRight = 283,
    MetalSilver = 284,
    MetalCopper = 285,
    MetalGold = 286,
    GenericStripedHazardYellow = 287,
    GenericStripedHazardBlack = 288,
    GenericYellowFace = 289,
    GenericYellowFaceSmile = 290,
    GenericYellowFaceFrown = 291,
    GenericYellow = 292,
    GenericBlack = 293,
    GenericBlackTransparent = 294,
    SecretAppear = 295,
    SecretDisappear = 296,
    SecretInvisible = 297,
    GlassRed = 298,
    GlassOrange = 299,
    GlassYellow = 300,
    GlassGreen = 301,
    GlassCyan = 302,
    GlassBlue = 303,
    GlassPurple = 304,
    GlassMagenta = 305,
    MineralsRed = 306,
    MineralsOrange = 307,
    MineralsYellow = 308,
    MineralsGreen = 309,
    MineralsCyan = 310,
    MineralsBlue = 311,
    MineralsPurple = 312,
    MineralsMagenta = 313,
    FactoryCrateMetal = 314,
    FactoryCrateWood = 315,
    FactoryStone = 316,
    FactoryWood = 317,
    FactoryScales = 318,
    MeadowGrassLeft = 319,
    MeadowGrassMiddle = 320,
    MeadowGrassRight = 321,
    MeadowBushLeft = 322,
    MeadowBushMiddle = 323,
    MeadowBushRight = 324,
    MeadowYellowFlower = 325,
    MeadowSmallBush = 326,
    EasterEggBlue = 327,
    EasterEggPink = 328,
    EasterEggYellow = 329,
    EasterEggRed = 330,
    EasterEggGreen = 331,
    CandyPink = 332,
    CandyBlue = 333,
    CandyOnewayPinkTop = 334,
    CandyOnewayRedTop = 335,
    CandyOnewayCyanTop = 336,
    CandyOnewayGreenTop = 337,
    CandyCane = 338,
    CandyLicorice = 339,
    CandyChocolate = 340,
    CandyCreamSmall = 341,
    CandyCreamLarge = 342,
    CandyGumdropRed = 343,
    CandyGumdropGreen = 344,
    CandyGumdropPink = 345,
    CandyPinkBg = 346,
    CandyBlueBg = 347,
    BeachSand = 348,
    BeachParasol = 349,
    BeachSandDriftBottomLeft = 350,
    BeachSandDriftTopLeft = 351,
    BeachSandDriftTopRight = 352,
    BeachSandDriftBottomRight = 353,
    BeachRock = 354,
    BeachDryBush = 355,
    SummerBeach = 356,
    SummerPail = 357,
    SummerShovel = 358,
    SummerDrink = 359,
    BeachLifePreserverRingRed = 360,
    BeachAnchor = 361,
    BeachDockRopeLeft = 362,
    BeachDockRopeRight = 363,
    BeachTreePalm = 364,
    JungleFaceBlock = 365,
    JungleOnewayTop = 366,
    JungleGray = 367,
    JungleRed = 368,
    JungleBlue = 369,
    JungleOlive = 370,
    JunglePot = 371,
    JunglePlant = 372,
    JunglePotBroken = 373,
    JungleStatue = 374,
    JungleGrayBg = 375,
    JungleRedBg = 376,
    JungleBlueBg = 377,
    JungleOliveBg = 378,
    JungleLeavesLightBg = 379,
    JungleLeavesMediumBg = 380,
    JungleLeavesDarkBg = 381,
    EnvironmentLog = 382,
    EnvironmentGrass = 383,
    EnvironmentBamboo = 384,
    EnvironmentObsidian = 385,
    EnvironmentLava = 386,
    EnvironmentLogBg = 387,
    EnvironmentGrassBg = 388,
    EnvironmentBambooBg = 389,
    EnvironmentObsidianBg = 390,
    EnvironmentLavaBg = 391,
    WindowClear = 392,
    WindowRed = 393,
    WindowOrange = 394,
    WindowYellow = 395,
    WindowGreen = 396,
    WindowTeal = 397,
    WindowBlue = 398,
    WindowPurple = 399,
    WindowPink = 400,
    CanvasRed = 401,
    CanvasBlue = 402,
    CanvasGreen = 403,
    CanvasGrayBg = 404,
    CanvasRedBg = 405,
    CanvasOrangeBg = 406,
    CanvasTanBg = 407,
    CanvasYellowBg = 408,
    CanvasGreenBg = 409,
    CanvasCyanBg = 410,
    CanvasBlueBg = 411,
    CanvasPurpleBg = 412,
    StoneGray = 413,
    StoneGreen = 414,
    StoneBrown = 415,
    StoneBlue = 416,
    StoneGrayBg = 417,
    StoneHalfGrayBg = 418,
    StoneGreenBg = 419,
    StoneHalfGreenBg = 420,
    StoneBrownBg = 421,
    StoneHalfBrownBg = 422,
    StoneBlueBg = 423,
    StoneHalfBlueBg = 424,
    GemstoneGreen = 425,
    GemstonePurple = 426,
    GemstoneYellow = 427,
    GemstoneBlue = 428,
    GemstoneRed = 429,
    GemstoneCyan = 430,
    GemstoneWhite = 431,
    GemstoneBlack = 432,
    SandWhite = 433,
    SandWhiteSurface = 434,
    SandGray = 435,
    SandGraySurface = 436,
    SandYellow = 437,
    SandYellowSurface = 438,
    SandOrange = 439,
    SandOrangeSurface = 440,
    SandBrownLight = 441,
    SandBrownLightSurface = 442,
    SandBrownDark = 443,
    SandBrownDarkSurface = 444,
    SandYellowLightBg = 445,
    SandGrayBg = 446,
    SandYellowDarkBg = 447,
    SandOrangeBg = 448,
    SandBrownLightBg = 449,
    SandBrownDarkBg = 450,
    FarmWheat = 451,
    FarmCorn = 452,
    FarmFenceLeft = 453,
    FarmFenceMiddle = 454,
    FarmFenceRight = 455,
    FarmHay = 456,
    CaveGrayLightBg = 457,
    CaveGrayDarkBg = 458,
    CaveBlackBg = 459,
    CaveRedBg = 460,
    CaveBrownBg = 461,
    CaveYellowBg = 462,
    CaveGreenBg = 463,
    CaveCyanBg = 464,
    CaveBlueBg = 465,
    CavePurpleBg = 466,
    CavePinkBg = 467,
    HalloweenBlood = 468,
    HalloweenBrickGray = 469,
    HalloweenGrayBg = 470,
    HalloweenBrickGrayBg = 471,
    HalloweenBrickGrayRightBg = 472,
    HalloweenBrickGrayLeftBg = 473,
    HalloweenTreeBg = 474,
    HalloweenLeavesPurpleBg = 475,
    HalloweenTombstone = 476,
    HalloweenCobwebBottomLeft = 477,
    HalloweenCobwebTopLeft = 478,
    HalloweenCobwebTopRight = 479,
    HalloweenCobwebBottomRight = 480,
    HalloweenTreeBranchBottomLeft = 481,
    HalloweenTreeBranchTopLeft = 482,
    HalloweenTreeBranchTopRight = 483,
    HalloweenTreeBranchBottomRight = 484,
    HalloweenPumpkinOn = 485,
    HalloweenPumpkinOff = 486,
    HalloweenGrassPurple = 487,
    HalloweenEyesYellow = 488,
    HalloweenEyesOrange = 489,
    HalloweenEyesPurple = 490,
    HalloweenEyesGreen = 491,
    MarbleColumnTop = 492,
    MarbleColumnMiddle = 493,
    MarbleColumnBottom = 494,
    MarbleGray = 495,
    MarbleGreen = 496,
    MarbleRed = 497,
    MarbleOneway = 498,
    MarbleGrayBg = 499,
    MarbleGreenBg = 500,
    MarbleRedBg = 501,
    WildwestOnewayBrownTop = 502,
    WildwestOnewayBrownDarkTop = 503,
    WildwestOnewayRedTop = 504,
    WildwestOnewayRedDarkTop = 505,
    WildwestOnewayBlueTop = 506,
    WildwestOnewayBlueDarkTop = 507,
    WildwestSidingBrownLightBg = 508,
    WildwestSidingBrownDarkBg = 509,
    WildwestSidingRedLightBg = 510,
    WildwestSidingRedDarkBg = 511,
    WildwestSidingBlueLightBg = 512,
    WildwestSidingBlueDarkBg = 513,
    WildwestPoleWhiteHorizontal = 514,
    WildwestPoleWhiteVertical = 515,
    WildwestPoleGrayHorizontal = 516,
    WildwestPoleGrayVertical = 517,
    WildwestDoorLeftBrown = 518,
    WildwestDoorLeftRed = 519,
    WildwestDoorLeftBlue = 520,
    WildwestDoorRightBrown = 521,
    WildwestDoorRightRed = 522,
    WildwestDoorRightBlue = 523,
    WildwestWindow = 524,
    WildwestFenceBrownLight = 525,
    WildwestFenceBrownDark = 526,
    WildwestFenceRedLight = 527,
    WildwestFenceRedDark = 528,
    WildwestFenceBlueLight = 529,
    WildwestFenceBlueDark = 530,
    NeonMagentaBg = 531,
    NeonOrangeBg = 532,
    NeonYellowBg = 533,
    NeonGreenBg = 534,
    NeonCyanBg = 535,
    NeonBlueBg = 536,
    CloudWhiteCenter = 537,
    CloudWhiteTop = 538,
    CloudWhiteBottom = 539,
    CloudWhiteLeft = 540,
    CloudWhiteRight = 541,
    CloudWhiteTopRight = 542,
    CloudWhiteTopLeft = 543,
    CloudWhiteBottomLeft = 544,
    CloudWhiteBottomRight = 545,
    CloudGrayCenter = 546,
    CloudGrayTop = 547,
    CloudGrayBottom = 548,
    CloudGrayLeft = 549,
    CloudGrayRight = 550,
    CloudGrayTopRight = 551,
    CloudGrayTopLeft = 552,
    CloudGrayBottomLeft = 553,
    CloudGrayBottomRight = 554,
    IndustrialIron = 555,
    IndustrialWires = 556,
    IndustrialPlateGrayPlainBg = 557,
    IndustrialPlateGrayGrayBg = 558,
    IndustrialPlateGrayBlueBg = 559,
    IndustrialPlateGrayGreenBg = 560,
    IndustrialPlateGrayYellowBg = 561,
    IndustrialOnewayGrayTop = 562,
    IndustrialScaffoldingHorizontal = 563,
    IndustrialScaffoldingVertical = 564,
    IndustrialPistonLeft = 565,
    IndustrialPistonTop = 566,
    IndustrialPistonRight = 567,
    IndustrialPistonBottom = 568,
    IndustrialPipeThickHorizontal = 569,
    IndustrialPipeThickVertical = 570,
    IndustrialPipeThinHorizontal = 571,
    IndustrialPipeThinVertical = 572,
    IndustrialConveyorLeft = 573,
    IndustrialConveyorMiddlePeg = 574,
    IndustrialConveyorMiddle = 575,
    IndustrialConveyorRight = 576,
    IndustrialCautionSignFire = 577,
    IndustrialCautionSignDeath = 578,
    IndustrialCautionSignElectricity = 579,
    IndustrialCautionSignEntry = 580,
    IndustrialCautionTapeHorizontal = 581,
    IndustrialCautionTapeVertical = 582,
    IndustrialPipeDecorationHorizontal = 583,
    IndustrialPipeDecorationVertical = 584,
    IndustrialLampOverhead = 585,
    IndustrialTeslaCoil = 586,
    IndustrialWireHorizontal = 587,
    IndustrialWireVertical = 588,
    IndustrialElectricity = 589,
    ClayTileWhiteBg = 590,
    ClayTileBrickBg = 591,
    ClayTileDiamondBg = 592,
    ClayTileXBg = 593,
    ClayTileRoughBg = 594,
    MedievalOnewayGrayTop = 595,
    MedievalBrick = 596,
    MedievalBrickWindow = 597,
    MedievalAnvil = 607,
    MedievalBarrel = 608,
    MedievalBlinds = 609,
    MedievalBrickBg = 598,
    MedievalWoodBg = 599,
    MedievalStrawBg = 600,
    MedievalRoofRedBg = 601,
    MedievalRoofGreenBg = 602,
    MedievalRoofBrownBg = 603,
    MedievalRoofGrayBg = 604,
    MedievalBrickDecorationTopMiddle = 605,
    MedievalStone = 606,
    MedievalScaffoldingStraightHorizontal = 610,
    MedievalScaffoldingStraightT = 611,
    MedievalScaffoldingStraightVertical = 612,
    MedievalScaffoldingAngledLeft = 613,
    MedievalScaffoldingAngledMiddle = 614,
    MedievalScaffoldingAngledRight = 615,
    MedievalAxeBottomLeft = 616,
    MedievalAxeTopLeft = 617,
    MedievalAxeTopRight = 618,
    MedievalAxeBottomRight = 619,
    MedievalSwordTopRight = 620,
    MedievalSwordBottomRight = 621,
    MedievalSwordBottomLeft = 622,
    MedievalSwordTopLeft = 623,
    MedievalShieldCircleRed = 624,
    MedievalShieldCircleBlue = 625,
    MedievalShieldCircleGreen = 626,
    MedievalShieldCircleYellow = 627,
    MedievalShieldCurvedRed = 628,
    MedievalShieldCurvedBlue = 629,
    MedievalShieldCurvedGreen = 630,
    MedievalShieldCurvedYellow = 631,
    MedievalBannerRed = 632,
    MedievalBannerBlue = 633,
    MedievalBannerGreen = 634,
    MedievalBannerYellow = 635,
    MedievalShield = 636,
    PipesLeft = 637,
    PipesUp = 638,
    PipesRight = 639,
    PipesDown = 640,
    PipesHorizontal = 641,
    PipesVertical = 642,
    OuterspaceWhite = 643,
    OuterspaceBlue = 644,
    OuterspaceGreen = 645,
    OuterspaceRed = 646,
    OuterspaceSand = 647,
    OuterspaceMoon = 648,
    OuterspaceMarsRock1 = 649,
    OuterspaceMarsRock2 = 650,
    OuterspaceMarsRock3 = 651,
    OuterspaceMarsRock4 = 652,
    OuterspaceMarsRock5 = 653,
    OuterspaceWhiteBg = 654,
    OuterspaceBlueBg = 655,
    OuterspaceGreenBg = 656,
    OuterspaceRedBg = 657,
    OuterspaceMarsRock3Bg = 658,
    OuterspaceMarsRock4Bg = 659,
    OuterspaceMarsRock5Bg = 660,
    OuterspaceSign = 661,
    OuterspaceLightRed = 662,
    OuterspaceLightBlue = 663,
    OuterspaceLightGreen = 664,
    OuterspaceLightYellow = 665,
    OuterspaceLightMagenta = 666,
    OuterspaceLightCyan = 667,
    OuterspaceComputer = 668,
    OuterspaceStarRed = 669,
    OuterspaceStarBlue = 670,
    OuterspaceStarYellow = 671,
    OuterspaceRockGray = 672,
    GardenRock = 673,
    GardenGrass = 674,
    GardenLeaves = 675,
    GardenGrassplant = 676,
    GardenFence = 677,
    GardenLattice = 678,
    GardenOnewayLeafBranch = 679,
    GardenOnewayLeafLeft = 680,
    GardenOnewayLeafRight = 681,
    GardenSnail = 682,
    GardenButterfly = 683,
    GardenRockBg = 684,
    GardenGrassBg = 685,
    GardenLeavesBg = 686,
    GardenFrame = 687,
    DomesticTile = 688,
    DomesticWood = 689,
    DomesticWoodPanel = 690,
    DomesticWallpaperYellowBg = 691,
    DomesticWallpaperBrownBg = 692,
    DomesticWallpaperRedBg = 693,
    DomesticWallpaperBlueBg = 694,
    DomesticWallpaperGreenBg = 695,
    DomesticLamp = 696,
    DomesticLightBulbBottomOff = 697,
    DomesticLightBulbBottomOn = 698,
    DomesticLightBulbTopOff = 699,
    DomesticLightBulbTopOn = 700,
    DomesticPipeBottomLeft = 701,
    DomesticPipeTopLeft = 702,
    DomesticPipeTopRight = 703,
    DomesticBottomRight = 704,
    DomesticPipeCross = 705,
    DomesticPipeStraightHorizontal = 706,
    DomesticPipeStraightVertical = 707,
    DomesticPipeTBottom = 708,
    DomesticPipeTLeft = 709,
    DomesticPipeTTop = 710,
    DomesticPipeTRight = 711,
    DomesticPaintingPurple = 712,
    DomesticPaintingBlue = 713,
    DomesticPaintingBlueDark = 714,
    DomesticPaintingGreen = 715,
    DomesticVaseBlue = 716,
    DomesticVasePurple = 717,
    DomesticVaseOrange = 718,
    DomesticVaseYellow = 719,
    DomesticTelevisionBlack = 720,
    DomesticTelevisionBlue = 721,
    DomesticTelevisionGray = 722,
    DomesticTelevisionYellow = 723,
    DomesticWindowBlack = 724,
    DomesticWindowBlue = 725,
    DomesticWindowOrange = 726,
    DomesticWindowYellow = 727,
    DomesticHalfYellowLeft = 728,
    DomesticHalfYellowTop = 729,
    DomesticHalfYellowRight = 730,
    DomesticHalfYellowBottom = 731,
    DomesticHalfBrownLeft = 732,
    DomesticHalfBrownTop = 733,
    DomesticHalfBrownRight = 734,
    DomesticHalfBrownBottom = 735,
    DomesticHalfWhiteLeft = 736,
    DomesticHalfWhiteTop = 737,
    DomesticHalfWhiteRight = 738,
    DomesticHalfWhiteBottom = 739,
    DomesticFrameBorderFull = 740,
    DomesticFrameBorderLeft = 741,
    DomesticFrameBorderTop = 742,
    DomesticFrameBorderRight = 743,
    DomesticFrameBorderBottom = 744,
    DomesticFrameBorderTopLeft = 745,
    DomesticFrameBorderTopRight = 746,
    DomesticFrameBorderBottomLeft = 747,
    DomesticFrameBorderBottomRight = 748,
    DomesticFrameBorderLeftRight = 749,
    DomesticFrameBorderTopBottom = 750,
    DojoOnewayWhiteTop = 751,
    DojoOnewayGrayTop = 752,
    DojoWallpaperWhiteBg = 753,
    DojoWallpaperGrayBg = 754,
    DojoShinglesBlueBg = 755,
    DojoShinglesBlueDarkBg = 756,
    DojoShinglesRedBg = 757,
    DojoShinglesRedDarkBg = 758,
    DojoShinglesGreenBg = 759,
    DojoShinglesGreenDarkBg = 760,
    DojoRooftopBlueLeft = 761,
    DojoRooftopBlueDarkLeft = 762,
    DojoRooftopRedLeft = 763,
    DojoRooftopRedDarkLeft = 764,
    DojoRooftopGreenLeft = 765,
    DojoRooftopGreenDarkLeft = 766,
    DojoRooftopBlueRight = 767,
    DojoRooftopBlueDarkRight = 768,
    DojoRooftopRedRight = 769,
    DojoRooftopRedDarkRight = 770,
    DojoRooftopGreenRight = 771,
    DojoRooftopGreenDarkRight = 772,
    DojoWindowBright = 773,
    DojoWindowDark = 774,
    DojoChineseCharacterAnd = 775,
    DojoChineseCharacterBook = 776,
    DojoChineseSymbolYinYang = 777,
    ChristmasTreePlain = 778,
    ChristmasTreeLights = 779,
    ChristmasFenceSnow = 780,
    ChristmasFencePlain = 781,
    ChristmasOrnamentRed = 782,
    ChristmasOrnamentGreen = 783,
    ChristmasOrnamentBlue = 784,
    ChristmasWreath = 785,
    ChristmasStar = 786,
    ChristmasWrappingPaperYellowBg = 787,
    ChristmasWrappingPaperGreenBg = 788,
    ChristmasWrappingPaperBlueBg = 789,
    ChristmasRibbonBlueVertical = 790,
    ChristmasRibbonBlueHorizontal = 791,
    ChristmasRibbonBlueCross = 792,
    ChristmasRibbonPurpleVertical = 793,
    ChristmasRibbonPurpleHorizontal = 794,
    ChristmasRibbonPurpleCross = 795,
    ChristmasCandyCane = 796,
    ChristmasMistletoe = 797,
    ChristmasStocking = 798,
    ChristmasRibbonRedBow = 799,
    ChristmasGiftFullRed = 800,
    ChristmasGiftHalfRed = 801,
    ChristmasGiftFullGreen = 802,
    ChristmasGiftHalfGreen = 803,
    ChristmasGiftFullWhite = 804,
    ChristmasGiftHalfWhite = 805,
    ChristmasGiftFullBlue = 806,
    ChristmasGiftHalfBlue = 807,
    ChristmasGiftFullYellow = 808,
    ChristmasGiftHalfYellow = 809,
    ChristmasStringLightBottomRed = 810,
    ChristmasStringLightBottomGreen = 811,
    ChristmasStringLightBottomBlue = 812,
    ChristmasStringLightBottomPurple = 813,
    ChristmasStringLightBottomYellow = 814,
    ChristmasStringLightTopRed = 815,
    ChristmasStringLightTopGreen = 816,
    ChristmasStringLightTopBlue = 817,
    ChristmasStringLightTopPurple = 818,
    ChristmasStringLightTopYellow = 819,
    ChristmasBellYellow = 820,
    ChristmasBellGroupRed = 821,
    ChristmasCandleRed = 822,
    ScifiPanelRed = 823,
    ScifiPanelBlue = 824,
    ScifiPanelGreen = 825,
    ScifiPanelYellow = 826,
    ScifiPanelMagenta = 827,
    ScifiPanelCyan = 828,
    ScifiMetalGray = 829,
    ScifiMetalWhite = 830,
    ScifiBrownLeopard = 831,
    ScifiOnewayRedTop = 832,
    ScifiOnewayBlueTop = 833,
    ScifiOnewayGreenTop = 834,
    ScifiOnewayYellowTop = 835,
    ScifiOnewayMagentaTop = 836,
    ScifiOnewayCyanTop = 837,
    ScifiLaserBlueStraightHorizontal = 838,
    ScifiLaserBlueStraightVertical = 839,
    ScifiLaserBlueCornerTopleft = 841,
    ScifiLaserBlueCornerTopright = 840,
    ScifiLaserBlueCornerBottomright = 842,
    ScifiLaserBlueCornerBottomleft = 843,
    ScifiLaserGreenStraightHorizontal = 844,
    ScifiLaserGreenStraightVertical = 845,
    ScifiLaserGreenCornerTopleft = 847,
    ScifiLaserGreenCornerTopright = 846,
    ScifiLaserGreenCornerBottomright = 848,
    ScifiLaserGreenCornerBottomleft = 849,
    ScifiLaserOrangeStraightHorizontal = 850,
    ScifiLaserOrangeStraightVeritical = 851,
    ScifiLaserOrangeCornerTopleft = 853,
    ScifiLaserOrangeCornerTopright = 852,
    ScifiLaserOrangeCornerBottomright = 854,
    ScifiLaserOrangeCornerBottomleft = 855,
    ScifiLaserRedStraightHorizontal = 856,
    ScifiLaserRedStraightVertical = 857,
    ScifiLaserRedCornerTopleft = 859,
    ScifiLaserRedCornerTopright = 858,
    ScifiLaserRedCornerBottomright = 860,
    ScifiLaserRedCornerBottomleft = 861,
    ScifiOutlineGrayBg = 862,
    PrisonBars = 863,
    PrisonBrick = 864,
    PrisonBrickBg = 865,
    PrisonWindowOrangeBg = 866,
    PrisonWindowBlueBg = 867,
    PrisonWindowBlackBg = 868,
    PirateWoodPlankBrown = 869,
    PirateChestBrown = 870,
    PirateOnewayBrownTop = 871,
    PirateShipBorderBrown = 872,
    PirateSkeletonHead = 873,
    PirateCannon = 874,
    PiratePortWindow = 875,
    PirateWoodPlankLightBrownBg = 876,
    PirateWoodPlankBrownBg = 877,
    PirateWoodPlankDarkBrownBg = 878,
    PirateSkeletonFlagBg = 879,
    PlasticRed = 880,
    PlasticOrange = 881,
    PlasticYellow = 882,
    PlasticLime = 883,
    PlasticGreen = 884,
    PlasticCyan = 885,
    PlasticBlue = 886,
    PlasticMagenta = 887,
    LavaYellow = 888,
    LavaOrange = 889,
    LavaDarkOrange = 890,
    LavaYellowBg = 891,
    LavaOrangeBg = 892,
    LavaDarkRedBg = 893,
    GoldBasic = 894,
    GoldBrick = 895,
    GoldChisled = 896,
    GoldTile = 897,
    GoldMantleOnewayTop = 898,
    GoldBasicBg = 899,
    GoldBrickBg = 900,
    GoldChisledBg = 901,
    ConstructionPlywood = 902,
    ConstructionGravel = 903,
    ConstructionCement = 904,
    ConstructionBeamRedHorizontalLeft = 905,
    ConstructionBeamRedHorizontalMiddle = 906,
    ConstructionBeamRedHorizontalRight = 907,
    ConstructionBeamRedVerticalTop = 908,
    ConstructionBeamRedVerticalMiddle = 909,
    ConstructionBeamRedVerticalBottom = 910,
    ConstructionCautionOrange = 911,
    ConstructionConeOrange = 912,
    ConstructionSignWarning = 913,
    ConstructionSignStop = 914,
    ConstructionHydrantFire = 915,
    ConstructionPlywoodBg = 916,
    ConstructionGravelBg = 917,
    ConstructionCementBg = 918,
    ConstructionBeamRedHorizontalLeftBg = 919,
    ConstructionBeamRedHorizontalMiddleBg = 920,
    ConstructionBeamRedHorizontalRightBg = 921,
    ConstructionBeamRedVerticalTopBg = 922,
    ConstructionBeamRedVerticalMiddleBg = 923,
    ConstructionBeamRedVerticalBottomBg = 924,
    TilesWhite = 925,
    TilesGray = 926,
    TilesBlack = 927,
    TilesRed = 928,
    TilesOrange = 929,
    TilesYellow = 930,
    TilesGreen = 931,
    TilesCyan = 932,
    TilesBlue = 933,
    TilesPurple = 934,
    TilesWhiteBg = 935,
    TilesGrayBg = 936,
    TilesBlackBg = 937,
    TilesRedBg = 938,
    TilesOrangeBg = 939,
    TilesYellowBg = 940,
    TilesGreenBg = 941,
    TilesCyanBg = 942,
    TilesBlueBg = 943,
    TilesPurpleBg = 944,
    HalfblocksWhiteLeft = 945,
    HalfblocksWhiteTop = 946,
    HalfblocksWhiteRight = 947,
    HalfblocksWhiteBottom = 948,
    HalfblocksGrayLeft = 949,
    HalfblocksGrayTop = 950,
    HalfblocksGrayRight = 951,
    HalfblocksGrayBottom = 952,
    HalfblocksBlackLeft = 953,
    HalfblocksBlackTop = 954,
    HalfblocksBlackRight = 955,
    HalfblocksBlackBottom = 956,
    HalfblocksRedLeft = 957,
    HalfblocksRedTop = 958,
    HalfblocksRedRight = 959,
    HalfblocksRedBottom = 960,
    HalfblocksOrangeLeft = 961,
    HalfblocksOrangeTop = 962,
    HalfblocksOrangeRight = 963,
    HalfblocksOrangeBottom = 964,
    HalfblocksYellowLeft = 965,
    HalfblocksYellowTop = 966,
    HalfblocksYellowRight = 967,
    HalfblocksYellowBottom = 968,
    HalfblocksGreenLeft = 969,
    HalfblocksGreenTop = 970,
    HalfblocksGreenRight = 971,
    HalfblocksGreenBottom = 972,
    HalfblocksCyanLeft = 973,
    HalfblocksCyanTop = 974,
    HalfblocksCyanRight = 975,
    HalfblocksCyanBottom = 976,
    HalfblocksBlueLeft = 977,
    HalfblocksBlueTop = 978,
    HalfblocksBlueRight = 979,
    HalfblocksBlueBottom = 980,
    HalfblocksMagentaLeft = 981,
    HalfblocksMagentaTop = 982,
    HalfblocksMagentaRight = 983,
    HalfblocksMagentaBottom = 984,
    WinterIceBrick = 985,
    WinterSnow = 986,
    WinterGlacier = 987,
    WinterSlate = 988,
    WinterIce = 989,
    WinterOnewayIce = 990,
    WinterIceLight = 991,
    WinterIceDark = 992,
    WinterIceDarkLeft = 993,
    WinterIceDarkMiddle = 994,
    WinterIceDarkRight = 995,
    WinterHalfSnowLeft = 996,
    WinterHalfSnowTop = 997,
    WinterHalfSnowRight = 998,
    WinterHalfSnowBottom = 999,
    WinterHalfIceLeft = 1000,
    WinterHalfIceTop = 1001,
    WinterHalfIceRight = 1002,
    WinterHalfIceBottom = 1003,
    WinterIceSlippery = 1004,
    WinterSnowPile = 1005,
    WinterIceDriftBottomLeft = 1006,
    WinterIceDriftTopLeft = 1007,
    WinterIceDriftTopRight = 1008,
    WinterIceDriftBottomRight = 1009,
    WinterSnowDriftBottomLeft = 1010,
    WinterSnowDriftTopLeft = 1011,
    WinterSnowDriftTopRight = 1012,
    WinterSnowDriftBottomRight = 1013,
    WinterSnowFluffLeft = 1014,
    WinterSnowFluffMiddle = 1015,
    WinterSnowFluffRight = 1016,
    WinterSnowman = 1017,
    WinterTree = 1018,
    WinterSnowflakeLarge = 1019,
    WinterSnowflakeSmall = 1020,
    WinterIceDarkBg = 1021,
    WinterIceLightBg = 1022,
    WinterIceBrickBg = 1023,
    WinterSnowBg = 1024,
    WinterGlacierBg = 1025,
    WinterSlateBg = 1026,
    FairytalePebbles = 1027,
    FairytaleTreeOrange = 1028,
    FairytaleMossGreen = 1029,
    FairytaleCloudBlue = 1030,
    FairytaleMushroomBlockRed = 1031,
    FairytaleVineGreen = 1032,
    FairytaleMushroomDecorationOrange = 1033,
    FairytaleMushroomDecorationRed = 1034,
    FairytaleDewDrop = 1035,
    FairytaleMistOrangeBg = 1036,
    FairytaleMistGreenBg = 1037,
    FairytaleMistBlueBg = 1038,
    FairytaleMistPinkBg = 1039,
    FairytaleHalfOrangeLeft = 1040,
    FairytaleHalfOrangeTop = 1041,
    FairytaleHalfOrangeRight = 1042,
    FairytaleHalfOrangeBottom = 1043,
    FairytaleHalfGreenLeft = 1044,
    FairytaleHalfGreenTop = 1045,
    FairytaleHalfGreenRight = 1046,
    FairytaleHalfGreenBottom = 1047,
    FairytaleHalfBlueLeft = 1048,
    FairytaleHalfBlueTop = 1049,
    FairytaleHalfBlueRight = 1050,
    FairytaleHalfBlueBottom = 1051,
    FairytaleHalfPinkLeft = 1052,
    FairytaleHalfPinkTop = 1053,
    FairytaleHalfPinkRight = 1054,
    FairytaleHalfPinkBottom = 1055,
    FairytaleFlowerPink = 1056,
    FairytaleFlowerBlue = 1057,
    FairytaleFlowerOrange = 1058,
    SpringDirtBrown = 1059,
    SpringHedgeGreen = 1060,
    SpringDirtDriftBottomLeft = 1061,
    SpringDirtDriftTopLeft = 1062,
    SpringDirtDriftTopRight = 1063,
    SpringDirtDriftBottomRight = 1064,
    SpringDaisyPink = 1065,
    SpringDaisyWhite = 1066,
    SpringDaisyBlue = 1067,
    SpringTulipPink = 1068,
    SpringTulipRed = 1069,
    SpringTulipYellow = 1070,
    SpringDaffodilOrange = 1071,
    SpringDaffodilYellow = 1072,
    SpringDaffodilWhite = 1073,
    OnewayWhiteLeft = 1074,
    OnewayWhiteTop = 1075,
    OnewayWhiteRight = 1076,
    OnewayWhiteBottom = 1077,
    OnewayGrayLeft = 1078,
    OnewayGrayTop = 1079,
    OnewayGrayRight = 1080,
    OnewayGrayBottom = 1081,
    OnewayBlackLeft = 1082,
    OnewayBlackTop = 1083,
    OnewayBlackRight = 1084,
    OnewayBlackBottom = 1085,
    OnewayRedLeft = 1086,
    OnewayRedTop = 1087,
    OnewayRedRight = 1088,
    OnewayRedBottom = 1089,
    OnewayOrangeLeft = 1090,
    OnewayOrangeTop = 1091,
    OnewayOrangeRight = 1092,
    OnewayOrangeBottom = 1093,
    OnewayYellowLeft = 1094,
    OnewayYellowTop = 1095,
    OnewayYellowRight = 1096,
    OnewayYellowBottom = 1097,
    OnewayGreenLeft = 1098,
    OnewayGreenTop = 1099,
    OnewayGreenRight = 1100,
    OnewayGreenBottom = 1101,
    OnewayCyanLeft = 1102,
    OnewayCyanTop = 1103,
    OnewayCyanRight = 1104,
    OnewayCyanBottom = 1105,
    OnewayBlueLeft = 1106,
    OnewayBlueTop = 1107,
    OnewayBlueRight = 1108,
    OnewayBlueBottom = 1109,
    OnewayMagentaLeft = 1110,
    OnewayMagentaTop = 1111,
    OnewayMagentaRight = 1112,
    OnewayMagentaBottom = 1113,
    DungeonCobblestoneGrey = 1114,
    DungeonCobblestoneGreen = 1115,
    DungeonCobblestoneBlue = 1116,
    DungeonCobblestonePurple = 1117,
    DungeonCobblestoneGreyBg = 1118,
    DungeonCobblestoneGreenBg = 1119,
    DungeonCobblestoneBlueBg = 1120,
    DungeonCobblestonePurpleBg = 1121,
    DungeonPillarBottomPurple = 1122,
    DungeonPillarBottomGray = 1123,
    DungeonPillarBottomGreen = 1124,
    DungeonPillarBottomBlue = 1125,
    DungeonPillarMiddlePurple = 1126,
    DungeonPillarMiddleGray = 1127,
    DungeonPillarMiddleGreen = 1128,
    DungeonPillarMiddleBlue = 1129,
    DungeonOnewayPillarTopPurple = 1130,
    DungeonOnewayPillarTopGray = 1131,
    DungeonOnewayPillarTopGreen = 1132,
    DungeonOnewayPillarTopBlue = 1133,
    DungeonSteelArcLeftPurple = 1134,
    DungeonSteelArcLeftGray = 1135,
    DungeonSteelArcLeftGreen = 1136,
    DungeonSteelArcLeftBlue = 1137,
    DungeonPillarArcRightPurple = 1138,
    DungeonPillarArcRightGray = 1139,
    DungeonPillarArcRightGreen = 1140,
    DungeonPillarArcRightBlue = 1141,
    DungeonTorchPurple = 1142,
    DungeonTorchYellow = 1143,
    DungeonTorchBlue = 1144,
    DungeonTorchGreen = 1145,
    DungeonWindow = 1146,
    DungeonChainRing = 1147,
    DungeonChainHook = 1148,
    DungeonChainLock = 1149,
    RetailFlagPurple = 1150,
    RetailFlagRed = 1151,
    RetailFlagYellow = 1152,
    RetailFlagGreen = 1153,
    RetailFlagCyan = 1154,
    RetailFlagBlue = 1155,
    RetailAwningPurple = 1156,
    RetailAwningRed = 1157,
    RetailAwningYellow = 1158,
    RetailAwningGreen = 1159,
    RetailAwningCyan = 1160,
    RetailAwningBlue = 1161,
    SummerStraw = 1162,
    SummerPlankPurple = 1163,
    SummerPlankYelllow = 1164,
    SummerPlankGreen = 1165,
    SummerOnewayDockTop = 1166,
    SummerStrawBg = 1167,
    SummerPlankPurpleBg = 1168,
    SummerPlankYellowBg = 1169,
    SummerPlankGreenBg = 1170,
    SummerIceCreamMint = 1171,
    SummerIceCreamVanilla = 1172,
    SummerIceCreamChocolate = 1173,
    SummerIceCreamSrawberry = 1174,
    MineStoneBrown = 1175,
    MineStoneBrownBg = 1176,
    MineStalagmite = 1177,
    MineStalagtite = 1178,
    MineCrystalPurple = 1179,
    MineCrystalRed = 1180,
    MineCrystalYellow = 1181,
    MineCrystalGreen = 1182,
    MineCrystalCyan = 1183,
    MineCrystalBlue = 1184,
    MineTorch = 1185,
    HauntedMossyBrickGreen = 1186,
    HauntedSidingGray = 1187,
    HauntedMossyShinglesGray = 1188,
    HauntedOnewayStoneGrayTop = 1189,
    HauntedBushDead = 1190,
    HauntedFenceIron = 1191,
    HauntedWindowCurvedOff = 1192,
    HauntedWindowCurvedOn = 1193,
    HauntedWindowCircleOff = 1194,
    HauntedWindowCircleOn = 1195,
    HauntedLanternOff = 1196,
    HauntedLanternOn = 1197,
    HauntedMossyBrickGreenBg = 1198,
    HauntedSlidingGrayBg = 1199,
    HauntedMossyShinglesGrayBg = 1200,
    DesertRockOrange = 1201,
    DesertCactus = 1202,
    DesertBush = 1203,
    DesertTree = 1204,
    MonsterSkinGreenLightBg = 1205,
    MonsterSkinGreenDarkBg = 1206,
    MonsterScalesRedLightBg = 1207,
    MonsterScalesRedDarkBg = 1208,
    MonsterScalesPurpleLightBg = 1209,
    MonsterScalesPurpleDarkBg = 1210,
    MonsterTeethLargeLeft = 1211,
    MonsterTeethLargeTop = 1212,
    MonsterTeethLargeRight = 1213,
    MonsterTeethLargeBottom = 1214,
    MonsterTeethMediumLeft = 1215,
    MonsterTeethMediumTop = 1216,
    MonsterTeethMediumRight = 1217,
    MonsterTeethMediumBottom = 1218,
    MonsterTeethSmallLeft = 1219,
    MonsterTeethSmallTop = 1220,
    MonsterTeethSmallRight = 1221,
    MonsterTeethSmallBottom = 1222,
    MonsterEyePurple = 1223,
    MonsterEyeYellow = 1224,
    MonsterEyeBlue = 1225,
    SwampGrass = 1226,
    SwampLog = 1227,
    SwampSignToxic = 1228,
    SwampMudBg = 1229,
    SwampGrassBg = 1230,
    FallLeavesDriftBottomLeft = 1231,
    FallLeavesDriftTopLeft = 1232,
    FallLeavesDriftTopRight = 1233,
    FallLeavesDriftBottomRight = 1234,
    FallGrassLeft = 1235,
    FallGrassMiddle = 1236,
    FallGrassRight = 1237,
    FallAcorn = 1238,
    FallPumpkin = 1239,
    FallLeavesYellowBg = 1240,
    FallLeavesOrangeBg = 1241,
    FallLeavesRedBg = 1242,
    ValentinesHeartRed = 1243,
    ValentinesHeartPurple = 1244,
    ValentinesHeartPink = 1245,
    NewyearsHungLightRed = 1246,
    NewyearsHungLightYellow = 1247,
    NewyearsHungLightGreen = 1248,
    NewyearsHungLightBlue = 1249,
    NewyearsHungLightPink = 1250,
    NewyearsWine = 1251,
    NewyearsChampagne = 1252,
    NewyearsBalloonRed = 1253,
    NewyearsBalloonOrange = 1254,
    NewyearsBalloonGreen = 1255,
    NewyearsBalloonBlue = 1256,
    NewyearsBalloonPurple = 1257,
    NewyearsStreamerRed = 1258,
    NewyearsStreamerOrange = 1259,
    NewyearsStreamerGreen = 1260,
    NewyearsStreamerBlue = 1261,
    NewyearsStreamerPurple = 1262,
    LeprechaunShamrock = 1263,
    LeprechaunGoldPot = 1264,
    LeprechaunGoldBag = 1265,
    LeprechaunHorseShoe = 1266,
    LeprechaunRainbowLeft = 1267,
    LeprechaunRainbowRight = 1268,
    RestaurantFoodBurger = 1269,
    RestaurantFoodHotdog = 1270,
    RestaurantFoodSub = 1271,
    RestaurantFoodSoda = 1272,
    RestaurantFoodFries = 1273,
    RestaurantGlassWater = 1274,
    RestaurantGlassMilk = 1275,
    RestaurantGlassOrangejuice = 1276,
    RestaurantGlassEmpty = 1277,
    RestaurantPlateChicken = 1278,
    RestaurantPlateHam = 1279,
    RestaurantPlateFish = 1280,
    RestaurantPlateCookies = 1281,
    RestaurantPlatePieCherry = 1282,
    RestaurantPlateCakeChocolate = 1283,
    RestaurantPlateEmpty = 1284,
    RestaurantBowlSalad = 1285,
    RestaurantBowlSpaghetti = 1286,
    RestaurantBowlIcecream = 1287,
    RestaurantBowlEmpty = 1288,
    ToxicOnewayRustedLeft = 1289,
    ToxicOnewayRustedTop = 1290,
    ToxicOnewayRustedRight = 1291,
    ToxicOnewayRustedBottom = 1292,
    ToxicWasteBg = 1293,
    ToxicWasteBarrelOn = 1294,
    ToxicWasteBarrelOff = 1295,
    ToxicSewerDrainWaste = 1296,
    ToxicSewerDrainEmpty = 1297,
    ToxicSewerDrainWater = 1298,
    ToxicSewerDrainLava = 1299,
    ToxicSewerDrainMud = 1300,
    ToxicLadderVerticalBroken = 1301,
    ToxicRailRusted = 1302,
    TextileClothGreenBg = 1303,
    TextileClothBlueBg = 1304,
    TextileClothPinkBg = 1305,
    TextileClothYellowBg = 1306,
    TextileClothRedBg = 1307,
    CarnivalStripesRedWhiteBg = 1308,
    CarnivalStripesRedYellowBg = 1309,
    CarnivalStripesPurpleVioletBg = 1310,
    CarnivalCheckerBg = 1311,
    CarnivalPinkBg = 1312,
    CarnivalGreenBg = 1313,
    CarnivalYellowBg = 1314,
    CarnivalBlueBg = 1315,
    UnderwaterBg = 1316,
    UnderwaterOctopusBg = 1317,
    UnderwaterFishBg = 1318,
    UnderwaterSeahorseBg = 1319,
    UnderwaterSeaweedBg = 1320,
}
