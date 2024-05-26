using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.World;

namespace PixelPilot.Physics;

public class PhysicsBuilder
{
    private PixelWorld _world;
    private PixelPilotClient _client;

    public PhysicsBuilder()
    {
        
    }

    public PhysicsBuilder AttachClient(PixelPilotClient client)
    {
        _client = client;
        return this;
    }

    public PhysicsBuilder AttachWorld(PixelWorld world)
    {
        _world = world;
        return this;
    }

    public PhysicsSimulator Build()
    {
        
    }
}