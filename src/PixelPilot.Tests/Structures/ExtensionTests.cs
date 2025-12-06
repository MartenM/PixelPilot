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
    public void TestLegacyToChunked()
    {
        var airTest = new List<IPlacedBlock>()
        {
            new PlacedBlock(0, 0, WorldLayer.Background, new BasicBlock(PixelBlock.Empty)),
            new PlacedBlock(1, 0, WorldLayer.Background, new BasicBlock(PixelBlock.Empty)),
            new PlacedBlock(2, 0, WorldLayer.Background, new BasicBlock(PixelBlock.Empty)),
        };

        var packets = airTest.ToChunkedPackets();
        Assert.That(packets.Count, Is.EqualTo(1));
    }
}