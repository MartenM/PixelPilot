using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages.Send;
using PixelPilot.PixelGameClient.World.Constants;

namespace PixelPilotExample;

public static class PlatformUtil
{
    public static Thread GetThread(PixelPilotClient client)
    {
            var betaBlocks = new List<int>();
            betaBlocks.AddRange(Enumerable.Range((int) PixelBlock.BeveledWhite, 10));
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