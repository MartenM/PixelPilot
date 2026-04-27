using PixelPilot.Api;
using PixelPilot.Client;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.World;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Structures;
using PixelPilot.Structures.Converters;
using PixelPilot.Structures.Converters.PilotSimple;
using PixelPilot.Structures.Extensions;

namespace PixelGameTests.Structures;

public class PixelPilotStuctureTests
{
    private PixelPilotClient _client;
    private PixelWorld _world;
    private static Random _random = new();

    private static List<string> TestPopulatedWorlds =
    [
        "r082b210d67df52",
        "yzqm9vra06rqo80",
        "8xkpxqn1hj2danj",
        "legacy:PWAFxTxHVxa0I",
        "legacy:PWhhGENdqja0I"
    ];
    
    // Datasource for a list of stucture files.
    private static readonly string TestStructuresFolder = "Structures/TestFiles";
    public static IEnumerable<string> TestStructureFiles()
    {
        string folder = Path.Combine(TestContext.CurrentContext.TestDirectory, TestStructuresFolder);
        foreach (var file in Directory.GetFiles(folder, "*.json"))
        {
            var name = Path.GetFileName(file);
            if (name.StartsWith("DISABLED"))
            {
                continue;
            }
            
            yield return name;
        }
    }
    
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
        
        _world = new PixelWorld(_client);
        _client.OnPacketReceived += _world.HandlePacket;
    }

    [TestCaseSource(nameof(TestStructureFiles))]
    public async Task TestStuctureLoading(string fileName)
    {
        // Load file contents
        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, TestStructuresFolder, fileName);
        var contents = await File.ReadAllTextAsync(filePath);
        Assert.That(contents, Is.Not.Empty, $"File {fileName} should not be empty.");

        // Should go without errors.
        var structure = PilotSaveSerializer.Deserialize(contents);
        Assert.That(structure, Is.Not.Null, $"Output structure should not be null.");
        Assert.That(structure.Blocks, Is.Not.Empty, $"Should have more than zero blocks.");
    }

    [TestCaseSource(nameof(TestStructureFiles))]
    [Timeout(60000)]
    public async Task TestStucturePasting(string fileName)
    {
        // Load file contents
        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, TestStructuresFolder, fileName);
        var contents = await File.ReadAllTextAsync(filePath);
        var structure = PilotSaveSerializer.Deserialize(contents);
        
        // Create a client with an unsaved world for pasting test.
        await TestClientPasting(structure);
    }

    private async Task TestClientPasting(Structure structure)
    {
        var height = (int)Math.Ceiling((double)structure.Height / 25) * 25;
        var width = (int)Math.Ceiling((double)structure.Width / 25) * 25;

        var worldId = $"pixelpilot_nunit_{_random.Next(1000)}{1000}";
        await _client.Connect(worldId, new JoinData()
        {
            WorldHeight = height,
            WorldWidth = width,
            WorldTitle = $"[PixelPilot NUnit] {_random.Next(1000)}"
        });
        
        Assert.That(_client.IsConnected, Is.True, $"Required: Client should be connected.");
        
        // Send blocks 2 times if required.
        WorldExtensions.WorldDifference diff = _world.GetDifference(structure);
        List<IPlacedBlock> remainingBlocks = diff.Blocks;
        int packetsSend = 0;
        
        for (int attempt = 0; attempt < 2; attempt++)
        {
            // Send the blocks
            var packets = diff.AsPackets().ToList();
            packetsSend += packets.Count;
            
            _client.SendRange(packets);
            await _client.WaitForEmptyQueue();
            await Task.Delay(250);
            
            // Check remaining + exit early.
            diff = _world.GetDifference(structure);
            remainingBlocks = diff.Blocks;
            if (remainingBlocks.Count == 0)
            {
                break;
            }
        }

        var detailedError =
            remainingBlocks.Select(rb =>
            {
                var found = _world.BlockAt(rb.Layer, rb.X, rb.Y);
                var equal = rb.Block.Equals(found);
                return
                    $"Expected: {rb.Block.Block} Found: {found.Block} Equals: {equal}";
            });
        
        Assert.Multiple(() =>
        {
            Assert.That(packetsSend, Is.Not.EqualTo(0), $"Safety check: At least placed some blocks.");
            Assert.That(remainingBlocks.Count, Is.EqualTo(0), $"There are {remainingBlocks.Count} remaining blocks. Details:\n {string.Join("\n", detailedError)}");
            Assert.That(_client.IsConnected, Is.True, $"Client should not have disconnected.");
            Assert.That(_client.LastException, Is.Null, $"Client should have not generated any issues.");
        });
        
        await _client.Disconnect();
    }

    [TestCaseSource(nameof(TestStructureFiles))]
    [Timeout(60000)]
    public async Task TestMigration(string fileName)
    {
        // Load file contents
        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, TestStructuresFolder, fileName);
        var contents = await File.ReadAllTextAsync(filePath);
        var structure = PilotSaveSerializer.Deserialize(contents);
        
        Assert.That(structure, Is.Not.Null, $"Output structure should not be null.");
        var newSerializedString = PilotSaveSerializer.Serialize(structure);
        
        Assert.That(contents, Is.Not.Empty, $"Input structure should not be empty.");
        
        var toStuctureAgain = PilotSaveSerializer.Deserialize(newSerializedString);
        Assert.That(toStuctureAgain.Blocks.Count, Is.EqualTo(structure.Blocks.Count), $"Same amount of blocks.");

        await TestClientPasting(toStuctureAgain);
    }

    [TestCaseSource(nameof(TestPopulatedWorlds))]
    [Timeout(600000)]
    public async Task TestWorldParsing(string worldId)
    {
        var world = new PixelWorld(_client);
        _client.OnPacketReceived += world.HandlePacket;
        
        await _client.Connect(worldId);
        
        Assert.Multiple(() =>
        {
            Assert.That(_client.LastException, Is.Null, $"Exception thrown during connecting / world loading. {_client.LastException}");
            Assert.That(_client.IsConnected, Is.True, $"Required: Client should be connected.");
        });
        
        await Task.Delay(1000);
        await _client.Disconnect();
    }
}