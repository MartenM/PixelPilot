namespace PixelPilot.PixelGameClient.Messages.Queue;

public interface IPixelPacketQueue
{
    public void EnqueuePacket(IPixelGamePacketOut packet);
    public Task Start();
    public Task Stop();
}