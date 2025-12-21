using Google.Protobuf;
using PixelPilot.Api;
using PixelPilot.Client.Players;

namespace PixelPilot.Client.Abstract;

public interface IPixelPilotClient
{
    public int? BotId { get; }
    public string? Username { get; }

    public Task Connect(string roomId, JoinData? joinData = null);

    public Task Disconnect();

    public void Send(IMessage packet);

    public void SendRange(IEnumerable<IMessage> packets);

    public void SendChat(string msg, bool prefix = true);

    public void SendPm(string username, string msg, bool prefix = true);

    public void SendPm(IPixelPlayer player, string msg, bool prefix = false);
    
    public PixelApiClient GetApiClient();

}