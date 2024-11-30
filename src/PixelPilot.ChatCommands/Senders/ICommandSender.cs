using PixelPilot.Client.Players;

namespace PixelPilot.ChatCommands;

public interface ICommandSender
{
    public IPixelPlayer Player { get; }
    
    public void SendMessage(string msg);

    public bool HasPermission(string? permission);
}