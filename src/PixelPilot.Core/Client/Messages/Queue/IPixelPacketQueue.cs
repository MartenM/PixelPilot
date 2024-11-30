using Google.Protobuf;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Messages.Queue;

public interface IPixelPacketQueue : IDisposable
{
    public void EnqueuePacket(IMessage packet);

    public int QueueSize { get; }
    public Task Start();
    public Task Stop();
    public bool IsOwner { get; set; }
}