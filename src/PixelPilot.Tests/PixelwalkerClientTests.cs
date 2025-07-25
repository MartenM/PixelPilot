using PixelPilot.Client;

namespace PixelGameTests;

public class PixelwalkerClientTests
{
    private PixelPilotClient _client;
    
    [SetUp]
    public void Setup()
    {
        var email = Environment.GetEnvironmentVariable("PIXELWALKER_TEST_EMAIL");
        var password = Environment.GetEnvironmentVariable("PIXELWALKER_TEST_PASSWORD");

        if (email == null)
        {
            Assert.Fail("Missing environment variables: PIXELWALKER_TEST_EMAIL");
            return;
        }
        
        if (password == null)
        {
            Assert.Fail("Missing environment variables: PIXELWALKER_TEST_PASSWORD");
            return;
        }

        _client = PixelPilotClient.Builder()
            .SetEmail(email)
            .SetPassword(password)
            .SetPrefix("[CI/CD]")
            .SetAutomaticReconnect(false)
            .Build();
    }

    [Test(Description = "Test if we can join a regular world.")]
    public async Task TestJoinWorld()
    {
        await _client.Connect("teyi68i0plokzm1");
        
        Assert.That(_client.IsConnected, Is.True, "Client is not connected to world.");
    }
    
    [Test(Description = "Test if creating an unsaved world is successful.")]
    public async Task TestCreateUnsavedWorld()
    {
        // Random number to ensure we don't collide.
        var random = new Random();
        
        await _client.Connect($"pixelpilot_testing_" + random.Next(1000) + 1000, new JoinData()
        {
            WorldHeight = 100,
            WorldWidth = 100,
            WorldTitle = "[Sandbox]"
        });
        
        Assert.That(_client.IsConnected, Is.True, "Client is not connected to unsaved world.");
    }
}