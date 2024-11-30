using System.Drawing;
using PixelPilot.Client.Messages.Exceptions;

namespace PixelPilot.Client.Messages.Received;

public class OldChatMessagesPacket : IDynamicConstructedPacket
{
    public class ChatMessage {
        public string Username { get; set; }
        public string Message { get; set; }
        public Color ChatColor { get; set; }
        
        public ChatMessage(string username, string message, int chatColor)
        {
            Username = username;
            Message = message;
            ChatColor = Color.FromArgb(chatColor);
        }
    }
    
    public ChatMessage[] Messages { get; }
    public OldChatMessagesPacket(IReadOnlyList<dynamic> fields)
    {
        const int fieldsPerMessage = 3;
        if (fields.Count % fieldsPerMessage != 0)
        {
            throw new PacketConstructorDynamicException($"Fields should be divideable by {fieldsPerMessage}");
        }

        int totalMessages = fields.Count / fieldsPerMessage;
        Messages = new ChatMessage[totalMessages];

        for (int i = 0; i < totalMessages; i++)
        {
            Messages[i] = new ChatMessage(
                (string) fields[i * fieldsPerMessage],
                (string) fields[i * fieldsPerMessage + 1],
                (int) fields[i * fieldsPerMessage + 2]
            );
        }
    }
}