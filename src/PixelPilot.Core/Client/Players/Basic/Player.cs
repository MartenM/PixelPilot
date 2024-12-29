using System.Drawing;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.Client.Players.Basic;

public class Player : IPixelPlayer
{
    /// <summary>
    /// Construct a new player from the incoming packet.
    /// </summary>
    /// <param name="packet"></param>
    public Player(PlayerJoinedPacket packet)
    {
        Id = packet.Properties.PlayerId;
        AccountId = packet.Properties.AccountId;
        Username = packet.Properties.Username;
        Face = packet.Properties.Face;
        Role = packet.Properties.Role;
        CanGod = packet.Properties.Rights.CanGod;
        CanEdit = packet.Properties.Rights.CanEdit;
        X = packet.Properties.Position.X;
        Y = packet.Properties.Position.Y;
        GoldCoins = packet.WorldState.CoinsGold;
        BlueCoins = packet.WorldState.CoinsBlue;
        Deaths = packet.WorldState.Deaths;
        HasCrown = packet.WorldState.HasGoldCrown;
        HasCompletedWorld = packet.WorldState.HasSilverCrown;
        Godmode = packet.WorldState.Godmode;
        Modmode = packet.WorldState.Modmode;
    }

    public Player(int id, string accountId, string username, int face, string role, Color chatColor, double x, double y, int coins, int blueCoins, int deaths, bool godmode, bool modmode, bool hasCrown, bool canGod, bool canEdit)
    {
        Id = id;
        AccountId = accountId;
        Username = username;
        Face = face;
        Role = role;
        ChatColor = chatColor;
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
    public bool IsAdmin => Role.Equals("admin");
    public string Role { get; }
    public Color ChatColor { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public int GoldCoins { get; set; }
    public int BlueCoins { get; set; }
    public int Deaths { get; set; }
    public bool Godmode { get; set; }
    public bool Modmode { get; set; }
    public bool HasCrown { get; set; }
    public bool HasCompletedWorld { get; set; }
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