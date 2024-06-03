namespace PixelPilot.PixelGameClient.Messages.Queue;

public interface IPixelPacketQueue : IDisposable
{
    public void EnqueuePacket(IPixelGamePacketOut packet);

    public int QueueSize { get; }
    public Task Start();
    public Task Stop();
}