using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages.Send;

namespace PixelPilotExample;

public static class PlatformUtil
{
    public static Thread GetThread(PixelPilotClient client)
    {
            var betaBlocks = new List<int>();
            betaBlocks.AddRange(Enumerable.Range(62, 6));
            return new Thread(() =>
            {
                int currentIndex = 0;
                while (client.IsConnected)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        client.Send(new WorldBlockPlacedOutPacket(34 + x, 70, 1, betaBlocks[currentIndex]));
                        Thread.Sleep(25);
                    }

                    currentIndex = (currentIndex + 1) % betaBlocks.Count;
                }
            });
    }
}