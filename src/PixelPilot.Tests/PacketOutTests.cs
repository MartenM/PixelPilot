using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelGameTests;

/// <summary>
/// Tests for outbound packets.
/// </summary>
public class PacketOutTests
{
    [Test]
    public void PacketChatOutTest()
    {
        string messagA = "6b-06-00-01-41".ToUpper();
        string messagHelloWorld = "6b-06-00-0c-48-65-6c-6c-6f-20-77-6f-72-6c-64-21".ToUpper();
        
        var packetA = new PlayerChatOutPacket("A");
        var packetHelloWorld = new PlayerChatOutPacket("Hello world!");
        Assert.That(BitConverter.ToString(packetA.ToBinaryPacket()), Is.EqualTo(messagA), "Packets should be constructed properly.");
        Assert.That(BitConverter.ToString(packetHelloWorld.ToBinaryPacket()), Is.EqualTo(messagHelloWorld), "Packets should be constructed properly.");
    }

    [Test]
    public void PacketFaceOutTest()
    {
        string faceChange = "6b-0d-03-00-00-00-02".ToUpper();
        
        var packet = new PlayerFaceOutPacket(2);
        Assert.That(BitConverter.ToString(packet.ToBinaryPacket()), Is.EqualTo(faceChange), "Packets should be constructed properly.");
    }

    [Test]
    public void PacketPlayerMoveOutTest()
    {
        string move =
            "6b-0b-06-40-82-80-00-00-00-00-00-06-40-91-40-00-00-00-00-00-06-00-00-00-00-00-00-00-00-06-c0-4a-00-00-00-00-00-00-06-00-00-00-00-00-00-00-00-06-40-00-00-00-00-00-00-00-03-00-00-00-00-03-00-00-00-00-07-01-07-01-03-00-00-04-e1".ToUpper();
        
        var movePacket = new PlayerMoveOutPacket(592, 1104, 0, -52, 0, 2, 0, 0, true, true, 1249);
        Assert.That(BitConverter.ToString(movePacket.ToBinaryPacket()), Is.EqualTo(move), "Packets should be constructed properly.");
    }
}