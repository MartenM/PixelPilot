using PixelPilot.Client.Extensions;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.Placed;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;

namespace PixelGameTests.Structures;

public class ExtensionTests
{
    [Test]
    public void TestToChunked()
    {
        var airTest = new List<IPlacedBlock>()
        {
            new PlacedBlock(0, 0, WorldLayer.Background, new FlexBlock(PixelBlock.Empty)),
            new PlacedBlock(1, 0, WorldLayer.Background, new FlexBlock(PixelBlock.Empty)),
            new PlacedBlock(2, 0, WorldLayer.Background, new FlexBlock(PixelBlock.Empty)),
        };

        var packets = airTest.ToChunkedPackets();
        Assert.That(packets.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void TestToChunkedWithArgs()
    {
        var airTest = new List<IPlacedBlock>()
        {
            new PlacedBlock(0, 0, WorldLayer.Foreground, new FlexBlock(PixelBlock.CoinGoldDoor, new Dictionary<string, object>()
            {
                ["coins"] = 13,
            })),
            new PlacedBlock(1, 0, WorldLayer.Foreground, new FlexBlock(PixelBlock.CoinGoldDoor, new Dictionary<string, object>()
            {
                ["coins"] = 13,
            })),
            new PlacedBlock(2, 0, WorldLayer.Foreground, new FlexBlock(PixelBlock.CoinGoldDoor, new Dictionary<string, object>()
            {
                ["coins"] = 13,
            })),
        };

        var packets = airTest.ToChunkedPackets();
        Assert.That(packets.Count, Is.EqualTo(1));
    }
    
    [Test]
    public void TestToChunkedWithArgsNoMerge()
    {
        var airTest = new List<IPlacedBlock>()
        {
            new PlacedBlock(0, 0, WorldLayer.Foreground, new FlexBlock(PixelBlock.CoinGoldDoor, new Dictionary<string, object>()
            {
                ["coins"] = 13,
            })),
            new PlacedBlock(1, 0, WorldLayer.Foreground, new FlexBlock(PixelBlock.CoinGoldDoor, new Dictionary<string, object>()
            {
                ["coins"] = 14,
            })),
            new PlacedBlock(2, 0, WorldLayer.Foreground, new FlexBlock(PixelBlock.CoinGoldDoor, new Dictionary<string, object>()
            {
                ["coins"] = 15,
            })),
        };

        var packets = airTest.ToChunkedPackets();
        Assert.That(packets.Count, Is.EqualTo(3));
    }
}