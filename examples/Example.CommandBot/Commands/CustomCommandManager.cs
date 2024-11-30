using PixelPilot.ChatCommands;
using PixelPilot.Client;
using PixelPilot.Client.Players;
using PixelPilot.Client.Players.Basic;

namespace Example.CommandBot.Commands;

public class CustomCommandManager : PixelChatCommandManager<Player>
{
    private PixelPilotClient _client;
    
    public CustomCommandManager(PixelPilotClient client, PixelPlayerManager<Player> pixelPlayerManager) : base(client, pixelPlayerManager)
    {
        _client = client;
    }
    
    protected override ICommandSender CreateSender(Player player)
    {
        // Create a custom sender that executes the permission check.
        return new CustomSender(player, _client);
    }
}

class CustomSender : CommandSender
{
    public CustomSender(IPixelPlayer player, PixelPilotClient client) : base(player, client)
    {
        
    }

    public override bool HasPermission(string? permission)
    {
        // Allow by default if no permission is set.
        if (permission == null) return true;
        
        // Normally fetch the players rank here from the IPixelPlayer.
        var playerRank = Rank.Default;
        var permissions = playerRank.GetPermissions();
        return permissions.Contains(permission);
    }
}