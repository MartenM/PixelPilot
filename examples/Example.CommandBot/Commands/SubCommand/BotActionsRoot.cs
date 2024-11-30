using PixelPilot.ChatCommands.Commands;
using PixelPilot.Client;

namespace Example.CommandBot.Commands.SubCommand;

public class BotActionsRoot : RootCommand
{
    public BotActionsRoot(PixelPilotClient client) : base("bot", "Bot commands", "bot")
    {
        AddCommand(new DisconnectCommand(client));;
        AddCommand(new BroadcastCommand(client));;
    }
}