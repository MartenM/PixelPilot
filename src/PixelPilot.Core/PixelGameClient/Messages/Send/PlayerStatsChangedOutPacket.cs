using PixelPilot.PixelGameClient.Messages.Constants;

namespace PixelPilot.PixelGameClient.Messages.Send;

public class PlayerStatsChangedOutPacket : ReflectivePixelOutPacket
{
    public int GoldCoins { get; }
    public int BlueCoins { get; }
    public int DeathCount { get; }
    
    public PlayerStatsChangedOutPacket(int goldCoins, int blueCoins, int deathCount) : base(WorldMessageType.PlayerCounters)
    {
        GoldCoins = goldCoins;
        BlueCoins = blueCoins;
        DeathCount = deathCount;
    }
}