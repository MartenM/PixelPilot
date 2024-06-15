using System.Drawing;

namespace PixelPilot.PixelGameClient.Messages.Received;

public class OldChatMessagesPacket : IPixelGamePacket
{
    public OldChatMessagesPacket(string username, string message, int color)
    {
        Username = username;
        Message = message;
        ChatColor = Color.FromArgb(color);
    }

    public string Username { get; set; }
    public string Message { get; set; }
    public Color ChatColor { get; set; }
}