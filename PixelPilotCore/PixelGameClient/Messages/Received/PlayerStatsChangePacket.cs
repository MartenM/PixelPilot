namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerStatsChangePacket : IPixelGamePacket
{
    public PlayerStatsChangePacket(int id, int goldCoins, int blueCoins, int deathCount)
    {
        Id = id;
        GoldCoins = goldCoins;
        BlueCoins = blueCoins;
        DeathCount = deathCount;
    }

    public int Id { get; }
    public int GoldCoins { get; }
    public int BlueCoins { get; }
    public int DeathCount { get; }
}