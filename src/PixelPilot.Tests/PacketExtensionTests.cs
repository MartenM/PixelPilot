using Google.Protobuf;
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
        
        var initReceivedPacket = new PlayerInitReceivedPacket();
        var d = initReceivedPacket.ToWorldPacket();
        
        Assert.That(d.PacketCase, Is.EqualTo(WorldPacket.PacketOneofCase.PlayerInitReceived));
    }

    [Test]
    public void TestInvalidPacketConstruction()
    {
        var worldPacket = new WorldPacket();
        Assert.Throws<PacketWrapException>(() => worldPacket.ToWorldPacket());
    }
    
    [Test]
    public void TestPlayerIdExtractor()
    {
        var playerMovedPacket = (IMessage) new PlayerMovedPacket()
        {
            PlayerId = 10,
        };
        Assert.That(playerMovedPacket.GetPlayerId(), Is.EqualTo(10));

        
        var systemMessage = (IMessage) new SystemMessagePacket();
        Assert.That(systemMessage.GetPlayerId(), Is.EqualTo(null));
    }
}