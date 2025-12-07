using PixelPilot.Client;
using PixelPilot.Client.World.Blocks;
using PixelPilot.Client.World.Blocks.V2;
using PixelPilot.Client.World.Constants;

namespace Example.BasicBot;

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
                        client.Send(new FlexBlock(betaBlocks[currentIndex]).AsPacketOut(34 +x, 70, 1));
                        Thread.Sleep(25);
                    }

                    currentIndex = (currentIndex + 1) % betaBlocks.Count;
                }
            });
    }
}