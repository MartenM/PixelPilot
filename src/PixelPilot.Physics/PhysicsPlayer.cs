using System.Drawing;
using PixelPilot.PixelGameClient.Players;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilot.Physics;

/// <summary>
/// Wraps around a IPixelPlayer.
/// Used to perform calculations. The actual player is updated afterwards.
/// </summary>
public class PhysicsPlayer
{
    private IPixelPlayer _mirrorPlayer;

    public PhysicsPlayer(IPixelPlayer mirrorPlayer)
    {
        _mirrorPlayer = mirrorPlayer;
    }

    public void FromMirror()
    {
        X = _mirrorPlayer.X;
        Y = _mirrorPlayer.Y;
        SpeedX = _mirrorPlayer.VelocityX;
        SpeedY = _mirrorPlayer.VelocityY;
        Horizontal = _mirrorPlayer.Horizontal;
        Vertical = _mirrorPlayer.Vertical;
        ModifierX = _mirrorPlayer.ModX;
        ModifierY = _mirrorPlayer.ModY;
    }

    public void ToMirror()
    {
        _mirrorPlayer.X = X;
        _mirrorPlayer.Y = Y;
        _mirrorPlayer.VelocityX = SpeedX;
        _mirrorPlayer.VelocityY = SpeedY;
        _mirrorPlayer.Horizontal = Horizontal;
        _mirrorPlayer.Vertical = Vertical;
        _mirrorPlayer.ModX = ModifierX;
        _mirrorPlayer.ModY = ModifierY;
    }

    public int Id { get; set; } = 0;
    public Point? LastPortalPos { get; set; } = null;
    public double X { get; set; } = 0;
    public double Y { get; set; } = 0;
    public Dictionary<string, bool> ActiveKeys { get; set; } = new Dictionary<string, bool>
    {
        { "red", false },
        { "green", false },
        { "blue", false }
    };
    public double SpeedX { get; set; } = 0;
    public double SpeedY { get; set; } = 0;
    
    public double TempSpeedX { get; set; } = 0;
    public double TempSpeedY { get; set; } = 0;
    public double ModifierX { get; set; } = 0;
    public double ModifierY { get; set; } = 0;
    
    public double TempModifierX { get; set; } = 0;
    public double TempModifierY { get; set; } = 0;
    public double MorX { get; set; } = 0;
    public double MorY { get; set; } = 0;
    public double MoX { get; set; } = 0;
    public double MoY { get; set; } = 0;
    public double Mx { get; set; } = 0;
    public double My { get; set; } = 0;
    public double Ox { get; set; } = 0;
    public double Oy { get; set; } = 0;
    public object PastPos { get; set; } = null;
    public double ReminderX { get; set; } = 0;
    public double ReminderY { get; set; } = 0;
    public double CurrentSpeedX { get; set; } = 0;
    public double CurrentSpeedY { get; set; } = 0;
    public int Horizontal { get; set; } = 0;
    public int Vertical { get; set; } = 0;
    public bool SpaceDown { get; set; } = false;
    public bool SpaceJustDown { get; set; } = false;
    public float LastHorizontal { get; set; } = 0;
    public float LastVertical { get; set; } = 0;
    public bool LastSpaceDown { get; set; } = false;
    public bool LastSpaceJustDown { get; set; } = false;
    public bool JustTeleported { get; set; } = false;
    public long LastJump { get; set; } = 0;
    public int TickId { get; set; } = 0;
    public Queue<PixelBlock> Queue { get; set; } = new(new List<PixelBlock>() { PixelBlock.Empty, PixelBlock.Empty} );
    public bool IsInGodMode { get; set; } = false;
    public bool IsInModMode { get; set; } = false;
    public bool IsFlying => IsInGodMode || IsInModMode;
    
    public bool IsDead { get; set; } = false;
    public bool IsRespawning { get; set; } = false;
    public int DeathFrame { get; set; } = 0;
    public int StaffAuraFrame { get; set; } = 0;
    public bool HasWinnerCrown { get; set; } = false;
    
    //public Counters Counters { get; set; }
    
    public byte[] Switches { get; set; } = new byte[1000];
    public byte[] DelayedLocalSwitches { get; set; } = new byte[1000];
    public byte[] DelayedGlobalSwitches { get; set; } = new byte[1000];
}