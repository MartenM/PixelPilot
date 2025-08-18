using PixelPilot.Client;
using PixelPilot.Structures;
using PixelPilot.Structures.Converters.PilotSimple;
using PixelPilot.Structures.Extensions;

namespace PixelGameTests.Structures;

public class PixelPilotStuctureTests
{
    private PixelPilotClient _client;
    private static Random _random = new Random();
    
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
    
    // This will be your data source: one entry per file
    public static IEnumerable<string> TestStructureFiles()
    {
        string folder = Path.Combine(TestContext.CurrentContext.TestDirectory, "Structures/TestFiles");
        foreach (var file in Directory.GetFiles(folder, "*.json"))
        {
            yield return file;
        }
    }

    [TestCaseSource(nameof(TestStructureFiles))]
    [Parallelizable(ParallelScope.All)]
    public async Task TestStuctureLoading(string filePath)
    {
        // Arrange
        var contents = await File.ReadAllTextAsync(filePath);
        Assert.That(contents, Is.Not.Empty, $"File {Path.GetFileName(filePath)} should not be empty.");

        // Should go without errors.
        var structure = PilotSaveSerializer.Deserialize(contents);
        Assert.That(structure, Is.Not.Null, $"Output structure should not be null.");
        Assert.That(structure.Blocks, Is.Not.Empty, $"Should have more than zero blocks.");
    }

    [TestCaseSource(nameof(TestStructureFiles))]
    [Timeout(600000)]
    public async Task TestStucturePasting(string filePath)
    {
        // Arrange
        var contents = await File.ReadAllTextAsync(filePath);
        var structure = PilotSaveSerializer.Deserialize(contents);
        
        // Create a client with an unsaved world for pasting test.
        var height = (int)Math.Ceiling((double)structure.Height / 25) * 25;
        var width = (int)Math.Ceiling((double)structure.Width / 25) * 25;
        
        await _client.Connect($"pixelpilot_nunit_{_random.Next(1000)}{1000}", new JoinData()
        {
            WorldHeight = height,
            WorldWidth = width,
            WorldTitle = $"[PixelPilot NUnit] {_random.Next(1000)}"
        });
        
        Assert.That(_client.IsConnected, Is.True, $"Required: Client should be connected.");

        var blockMessages = structure.BlocksWithEmpty.ToChunkedPackets();
        
        _client.SendRange(blockMessages);
        await _client.WaitForEmptyQueue();
        
        Assert.That(blockMessages, Is.Not.Empty, $"Client should not have disconnected.");
        await _client.Disconnect();
    }
}