using System.Drawing;

namespace PixelPilot.PixelGameClient.Players;

public interface IPixelPlayer
{
    /// <summary>
    /// The unique player ID of this player in this world.
    /// </summary>
    public int Id { get; }
    
    /// <summary>
    /// The account ID of this player.
    /// Use this for persistant storage.
    /// </summary>
    public string AccountId { get; }
    
    /// <summary>
    /// The display name of the player.
    /// </summary>
    public string Username { get;  }
    public int Face { get; set; }
    public bool IsAdmin { get; }
    
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
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
    public double ModX { get; set; }
    public double ModY { get; set; }
    public int Horizontal { get; set; }
    public int Vertical { get; set; }
    public bool Spacedown { get; set; }
    public bool SpaceJustDown { get; set; }
    public int TickId { get; set; }
    
    // Computed properties
    public int BlockX => (int)(X / 16 + 0.5);
    public int BlockY => (int)(Y / 16 + 0.5);
}