using PixelPilot.Client;
using PixelPilot.Client.Players;

namespace PixelPilot.ChatCommands;

public class CommandSender : ICommandSender
{
    private PixelPilotClient _client;
    
    public CommandSender(IPixelPlayer player, PixelPilotClient client, string prefixUsed)
    {
        Player = player;
        _client = client;
        PrefixUsed = prefixUsed;
    }

    public IPixelPlayer Player { get; }
    
    public string PrefixUsed { get; set; }

    public virtual void SendMessage(string msg)
    {
        _client.SendPm(Player.Username, msg);
    }

    public virtual bool HasPermission(string? permission)
    {
        if (permission == null) return true;
        return true;
    }
    
}