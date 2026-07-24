using Google.Protobuf;
using PixelPilot.Client.World.Zones;

namespace PixelGameTests;

public class ZoneMembershipRleTests
{
    [Test]
    public void TestRoundTripEmptyGrid()
    {
        var grid = new bool[5, 5];

        var encoded = MembershipRle.Encode(grid);
        var decoded = MembershipRle.Decode(encoded, 5, 5);

        AssertGridsEqual(grid, decoded);
    }

    [Test]
    public void TestRoundTripFullGrid()
    {
        var grid = new bool[4, 3];
        for (var x = 0; x < 4; x++)
        for (var y = 0; y < 3; y++)
            grid[x, y] = true;

        var encoded = MembershipRle.Encode(grid);
        var decoded = MembershipRle.Decode(encoded, 4, 3);

        AssertGridsEqual(grid, decoded);
    }

    [Test]
    public void TestRoundTripMixedGrid()
    {
        var grid = new bool[10, 10];
        for (var x = 2; x < 7; x++)
        for (var y = 3; y < 8; y++)
            grid[x, y] = true;

        var encoded = MembershipRle.Encode(grid);
        var decoded = MembershipRle.Decode(encoded, 10, 10);

        AssertGridsEqual(grid, decoded);
    }

    [Test]
    public void TestRoundTripCheckerboard()
    {
        var grid = new bool[6, 6];
        for (var x = 0; x < 6; x++)
        for (var y = 0; y < 6; y++)
            grid[x, y] = (x + y) % 2 == 0;

        var encoded = MembershipRle.Encode(grid);
        var decoded = MembershipRle.Decode(encoded, 6, 6);

        AssertGridsEqual(grid, decoded);
    }

    [Test]
    public void TestEncodeMatchesConfirmedWireFormat()
    {
        // width=1, height=3, column-major: (x0,y0)=false, (x0,y1)=false, (x0,y2)=true.
        // Expect: [startValue=0x00, run(2)=0x02, run(1)=0x01].
        var grid = new bool[1, 3];
        grid[0, 2] = true;

        var encoded = MembershipRle.Encode(grid);

        Assert.That(encoded.ToByteArray(), Is.EqualTo(new byte[] { 0x00, 0x02, 0x01 }));
    }

    [Test]
    public void TestDecodeMatchesConfirmedWireFormat()
    {
        var bytes = ByteString.CopyFrom(new byte[] { 0x01, 0x01, 0x02 });

        // width=1, height=3, start value true: (x0,y0)=true, then flip: (x0,y1)=(x0,y2)=false.
        var decoded = MembershipRle.Decode(bytes, 1, 3);

        Assert.That(decoded[0, 0], Is.True);
        Assert.That(decoded[0, 1], Is.False);
        Assert.That(decoded[0, 2], Is.False);
    }

    [Test]
    public void TestEmptyGridEncodesToZeroBytes()
    {
        var grid = new bool[0, 0];

        var encoded = MembershipRle.Encode(grid);

        Assert.That(encoded.IsEmpty, Is.True);
    }

    private static void AssertGridsEqual(bool[,] expected, bool[,] actual)
    {
        Assert.That(actual.GetLength(0), Is.EqualTo(expected.GetLength(0)));
        Assert.That(actual.GetLength(1), Is.EqualTo(expected.GetLength(1)));

        for (var x = 0; x < expected.GetLength(0); x++)
        {
            for (var y = 0; y < expected.GetLength(1); y++)
            {
                Assert.That(actual[x, y], Is.EqualTo(expected[x, y]), $"Mismatch at ({x},{y})");
            }
        }
    }
}
