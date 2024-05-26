namespace PixelPilot.Physics;

public class PhysicsConfig
{
    public int MsPerTick { get; set; } = 10;
    public int MaxMsPerTick { get; set; } = 15;
    public double VariableMultiplier { get; set; } = 7.752;
    public double Gravity { get; set; } = 2.0;
    public int JumpHeight { get; set; } = 26;
    public int BoostSpeed { get; set; } = 16;
    public DragConfig Drag { get; set; } = new();
    public BuoyancyConfig Buoyancy { get; set; } = new();
}

public class DragConfig
{
    public double Base { get; set; } = 0.9813195279915707;
    public double NoModifier { get; set; } = 0.9045276172161356;
    public double Ladder { get; set; } = 0.9045276172161356;
    public double Water { get; set; } = 0.95126319261906774922927297168877;
}

public class BuoyancyConfig
{
    public double Water { get; set; } = -0.5;
}