using PixelPilot.Api;
using PixelPilot.Api.Responses.Collections;

namespace PixelGameTests;

public class PixelwalkerApiTests
{
    private PixelApiClient _client;

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
        
        _client = new PixelApiClient(email, password);
    }
    
    [Test]
    public async Task TestGetPublicWorld()
    {
        var world =  await _client.GetPublicWorld("mknckr7oqxq24xa");
        Assert.NotNull(world);
    }
    
    [Test]
    public async Task TestGetWorlds()
    {
        // Test getting worlds of MartenM
        var world = await _client.GetOwnedWorlds(1, 10, new QueryArgumentBuilder().AddFilter("owner", "5tm26ynhcve12pc"));
        
        Assert.That(world, Is.Not.Null);
        Assert.That(world.Items.Capacity,  Is.GreaterThanOrEqualTo(2));
    }
    
    [Test]
    public async Task TestGetPublishedWorlds()
    {
        var query = new QueryArgumentBuilder()
            .AddFilter("difficulty", ">=", 1)
            .AddFilter("difficulty", "<=", 3);
        var worlds = await _client.GetPublishedWorlds(1, 10, query);

        Assert.That(worlds.Items, Is.Not.Empty);
        Assert.That(worlds.Items.All(w => w.Difficulty is >= 1 and <= 3), Is.True);
        Assert.That(worlds.Items[0].Expand?.World?.Expand.Owner, Is.Not.Null);
    }

    [Test]
    public async Task TestGetWorldLeaderboard()
    {
        var published = await _client.GetPublishedWorlds(1, 1, new QueryArgumentBuilder().SortDescending().SortBy("completions"));
        var leaderboard = await _client.GetWorldLeaderboard(published.Items[0].Id);

        Assert.That(leaderboard, Is.Not.Empty);
        Assert.That(leaderboard[0].Index, Is.EqualTo(1));
    }

    [Test]
    public async Task TestGetWorldLeaderboardWithWorldIdIsEmpty()
    {
        var published = await _client.GetPublishedWorlds(1, 1);
        var leaderboard = await _client.GetWorldLeaderboard(published.Items[0].World);

        Assert.That(leaderboard, Is.Empty);
    }

    [Test]
    public async Task TestGetPlayer()
    {
        // Test getting worlds of MartenM
        var player = await _client.GetPlayer("MARTEN");
        
        Assert.NotNull(player);
        Assert.That(player.Username, Is.EqualTo("MARTEN"));
    }
    
    [Test]
    public async Task TestGetMinimapInternals()
    {
        var world = await _client.GetPublicWorld("rayxnpc6oexzgf1");
        
        Assert.That(world, Is.Not.Null);
        
        var minimap = await _client.GetMinimap(world);
        
        Assert.NotNull(minimap);
        Assert.That(minimap.Length, Is.GreaterThan(0));
    }
    
    [Test]
    public async Task TestGetMinimap()
    {
        var world = await _client.GetPublicWorld("teyi68i0plokzm1");
        
        Assert.That(world, Is.Not.Null);
        
        var minimap1 = await _client.GetMinimap(world);
        var minimap2 = await _client.GetMinimap("teyi68i0plokzm1");
        
        Assert.NotNull(minimap1);
        Assert.That(minimap1.Length, Is.GreaterThan(0));
        
        Assert.NotNull(minimap2);
        Assert.That(minimap2.Length, Is.GreaterThan(0));
        
        Assert.That(minimap1, Is.EqualTo(minimap2));
    }
    
    [Test]
    public async Task TestGetGameVersion()
    {
        var version = await _client.GetGameVersion();
    }
}