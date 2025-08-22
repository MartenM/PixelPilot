﻿using PixelPilot.Client;
using PixelPilot.Client.Extensions;
using PixelPilot.Client.World;
using PixelPilot.Structures;
using PixelPilot.Structures.Converters.PilotSimple;
using PixelPilot.Structures.Extensions;

namespace PixelGameTests.Structures;

public class PixelPilotStuctureTests
{
    private PixelPilotClient _client;
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
            yield return Path.GetFileName(file);
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
    }

    [TestCaseSource(nameof(TestStructureFiles))]
    [Parallelizable(ParallelScope.All)]
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
    [Timeout(600000)]
    public async Task TestStucturePasting(string fileName)
    {
        // Load file contents
        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, TestStructuresFolder, fileName);
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
        
        Assert.Multiple(() =>
        {
            Assert.That(blockMessages, Is.Not.Empty, $"Safety check: At least placed some blocks.");
            Assert.That(_client.IsConnected, Is.True, $"Client should not have disconnected.");
            Assert.That(_client.LastException, Is.Null, $"Client should have not generated any issues.");
        });
        
        await _client.Disconnect();
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