using PixelPilot.Client.Messages.Packets.Exceptions;
using PixelPilot.Client.Messages.Packets.Extensions;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelGameTests;

public class PacketExtensionTests
{
    [Test]
    public void TestPacketExtraction()
    {
        var worldPacket = new WorldPacket();
        worldPacket.PlayerChatPacket = new PlayerChatPacket()
        {
            PlayerId = 1,
            Message = "Hello World"
        };
        
        
        var innerPacket = worldPacket.GetPacket();

        Assert.That(innerPacket is PlayerChatPacket, Is.EqualTo(true), "Packet type is not PlayerChatPacket");
    }

    [Test]
    public void TestPacketConstruction()
    {
        var initPacket = new PlayerInitPacket();
        var a = initPacket.ToWorldPacket();
        
        Assert.That(a.PacketCase, Is.EqualTo(WorldPacket.PacketOneofCase.PlayerInitPacket));
        
        var ping = new Ping();
        var b = ping.ToWorldPacket();
        
        Assert.That(b.PacketCase, Is.EqualTo(WorldPacket.PacketOneofCase.Ping));
        
        var facePacket = new PlayerFacePacket();
        var c = facePacket.ToWorldPacket();
        
        Assert.That(c.PacketCase, Is.EqualTo(WorldPacket.PacketOneofCase.PlayerFacePacket));
    }

    [Test]
    public void TestInvalidPacketConstruction()
    {
        var worldPacket = new WorldPacket();
        Assert.Throws<PacketWrapException>(() => worldPacket.ToWorldPacket());
    }
}