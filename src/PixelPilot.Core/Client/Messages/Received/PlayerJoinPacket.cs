using System.Drawing;

namespace PixelPilot.Client.Messages.Received;

public class PlayerJoinPacket : IPixelGamePlayerPacket
{
    public PlayerJoinPacket(int id, string cuid, string username, int face, bool isAdmin, bool isFriend, bool isOwner, bool hasGod, bool hasEdit, double x, double y, int chatColour, int coins, int blueCoins, int deaths, byte[] collectedItems, bool godmode, bool modmode, bool hasCrown, bool hasCompletedWorld, int team, byte[] switchBuffer)
    {
        PlayerId = id;
        Cuid = cuid;
        Username = username;
        Face = face;
        IsAdmin = isAdmin;
        IsFriend = isFriend;
        HasGod = hasGod;
        HasEdit = hasEdit;
        X = x;
        Y = y;
        ChatColor = Color.FromArgb(chatColour);
        Coins = coins;
        BlueCoins = blueCoins;
        Deaths = deaths;
        IsOwner = isOwner;
        Godmode = godmode;
        Modmode = modmode;
        HasCrown = hasCrown;
        HasCompletedWorld = hasCompletedWorld;
        SwitchBuffer = switchBuffer;
        Team = team;
        
        // Serialize the collected items
        Collected = new List<Point>();
        int collectedCount = collectedItems.Length / 4;
        for (int i = 0; i < collectedCount; i++)
        {
            int startIndex = i * 4;
            var collectedX = BitConverter.ToInt16(collectedItems, startIndex);
            var collectedY = BitConverter.ToInt16(collectedItems, startIndex + 2);
            Collected.Add(new Point(collectedX, collectedY));
        }
    }

    public int PlayerId { get; }
    public string Cuid { get; }
    public string Username { get; }
    public int Face { get; }
    public bool IsAdmin { get; }

    public bool IsFriend { get; }

    public Color ChatColor { get; set; }
    public bool HasGod { get; }
    public bool HasEdit { get; }
    public double X { get; }
    public double Y { get; }
    public int Coins { get; }
    public int BlueCoins { get; }
    public int Deaths { get; }
    
    public bool IsOwner { get; }
    public bool Godmode { get; }
    public bool Modmode { get; }
    public bool HasCrown { get; }
    
    public List<Point> Collected { get; }
    public int Team { get; }
    
    public bool HasCompletedWorld { get; }
    public byte[] SwitchBuffer { get; }
}