namespace PixelPilot.PixelGameClient.Messages.Received;

public class PlayerJoinPacket : IPixelGamePlayerPacket
{
    public PlayerJoinPacket(int id, string cuid, string username, int face, bool isAdmin, bool hasGod, bool hasEdit, double x, double y, int coins, int blueCoins, int deaths, bool godmode, bool modmode, bool hasCrown, byte[] buffer)
    {
        PlayerId = id;
        Cuid = cuid;
        Username = username;
        Face = face;
        IsAdmin = isAdmin;
        HasGod = hasGod;
        HasEdit = hasEdit;
        X = x;
        Y = y;
        Coins = coins;
        BlueCoins = blueCoins;
        Deaths = deaths;
        Godmode = godmode;
        Modmode = modmode;
        HasCrown = hasCrown;
        Buffer = buffer;
    }

    public int PlayerId { get; }
    public string Cuid { get; }
    public string Username { get; }
    public int Face { get; }
    public bool IsAdmin { get; }
    public bool HasGod { get; }
    public bool HasEdit { get; }
    public double X { get; }
    public double Y { get; }
    public int Coins { get; }
    public int BlueCoins { get; }
    public int Deaths { get; }
    public bool Godmode { get; }
    public bool Modmode { get; }
    public bool HasCrown { get; }
    public byte[] Buffer { get; }
}