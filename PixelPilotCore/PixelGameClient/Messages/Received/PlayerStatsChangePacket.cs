namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerStatsChangePacket : IPixelGamePacket
{
    public PlayerStatsChangePacket(int id, int goldCoins, int blueCoins, int deathCount)
    {
        PlayerId = id;
        GoldCoins = goldCoins;
        BlueCoins = blueCoins;
        DeathCount = deathCount;
    }

    public int PlayerId { get; }
    public int GoldCoins { get; }
    public int BlueCoins { get; }
    public int DeathCount { get; }
}