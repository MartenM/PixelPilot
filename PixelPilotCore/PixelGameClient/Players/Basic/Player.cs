using PixelPilot.PixelGameClient.Messages.Received;

namespace PixelPilot.PixelGameClient.Players.Basic;

public class Player : IPixelPlayer
{
    /// <summary>
    /// Construct a new player from the incoming packet.
    /// </summary>
    /// <param name="packet"></param>
    public Player(PlayerJoinPacket packet)
    {
        Id = packet.PlayerId;
        AccountId = packet.Cuid;
        Username = packet.Username;
        Face = packet.Face;
        IsAdmin = packet.IsAdmin;
        Godmode = packet.HasGod;
        CanEdit = packet.HasEdit;
        X = packet.X;
        Y = packet.Y;
        GoldCoins = packet.Coins;
        BlueCoins = packet.BlueCoins;
        Deaths = packet.Deaths;
        HasCrown = packet.HasCrown;
        Godmode = packet.Godmode;
        Modmode = packet.Modmode;
        CanGod = false;
        CanEdit = false;
    }

    public Player(int id, string accountId, string username, int face, bool isAdmin, double x, double y, int coins, int blueCoins, int deaths, bool godmode, bool modmode, bool hasCrown, bool canGod, bool canEdit)
    {
        Id = id;
        AccountId = accountId;
        Username = username;
        Face = face;
        IsAdmin = isAdmin;
        X = x;
        Y = y;
        GoldCoins = coins;
        BlueCoins = blueCoins;
        Deaths = deaths;
        Godmode = godmode;
        Modmode = modmode;
        HasCrown = hasCrown;
        CanGod = canGod;
        CanEdit = canEdit;
    }
    
    public int Id { get; }
    public string AccountId { get; }
    public string Username { get;  }
    public int Face { get; set; }
    public bool IsAdmin { get; }
    public double X { get; set; }
    public double Y { get; set; }
    public int GoldCoins { get; set; }
    public int BlueCoins { get; set; }
    public int Deaths { get; set; }
    public bool Godmode { get; set; }
    public bool Modmode { get; set; }
    public bool HasCrown { get; set; }
    public bool CanGod { get; set; }
    public bool CanEdit { get; set; }
    
    // Movement details
    public double VelocityX { get; set; } = 0;
    public double VelocityY { get; set; } = 0;
    public double ModX { get; set; } = 0;
    public double ModY { get; set; } = 0;
    public int Horizontal { get; set; } = 0;
    public int Vertical { get; set; } = 0;
    public bool Spacedown { get; set; } = false;
    public bool SpaceJustDown { get; set; } = false;
    public int TickId { get; set; } = 0;
}