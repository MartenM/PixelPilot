using Google.Protobuf;
using Microsoft.Extensions.Logging;
using PixelPilot.ChatCommands.Commands;
using PixelPilot.ChatCommands.Commands.Help;
using PixelPilot.ChatCommands.Messages;
using PixelPilot.Client;
using PixelPilot.Client.Messages;
using PixelPilot.Client.Players;
using PixelPilot.Common.Logging;
using PixelWalker.Networking.Protobuf.WorldPackets;

namespace PixelPilot.ChatCommands;

public class PixelChatCommandManager<T> : IChatCommandManager where T : IPixelPlayer
{
    private PixelPilotClient _client;
    private PixelPlayerManager<T> _pixelPlayerManager;
    private readonly ILogger _logger = LogManager.GetLogger("CommandManager");
    
    public List<string> CommandPrefixes { get; set; } = new() {"!", "."};
    public List<ChatCommand> ChatCommands { get; set; } = new();

    public IHelpFormatter HelpFormatter { get; set; } = new BasicHelpFormatter();
    
    public bool SendHelp { get; set; }
    public bool RegisterCustomCommands { get; set; } = true;
    public bool RegisterRestrictedCustomCommands { get; set; } = true;
    
    public List<ChatCommand> GetAvailableCommands(ICommandSender sender)
    {
        return ChatCommands.Where(cmd => cmd.CheckPermission(sender)).ToList();
    }

    public PixelChatCommandManager(PixelPilotClient client, PixelPlayerManager<T> pixelPlayerManager)
    {
        _client = client;
        _pixelPlayerManager = pixelPlayerManager;
        
        _client.OnPacketReceived += HandlePacket;
    }

    private void HandlePacket(object sender, IMessage packet)
    {
        if (packet is PlayerChatPacket chatPacket)
        {
            OnPlayerChat(chatPacket);
            return;
        }

        if (packet is PlayerDirectMessagePacket directMessagePacket)
        {
            OnDirectMessage(directMessagePacket);
            return;
        }
    }
    
    private void OnPlayerChat(PlayerChatPacket packet)
    {
        var prefix = CommandPrefixes.FirstOrDefault(prefix => packet.Message.StartsWith(prefix));
        if (prefix == null) return;
        
        var commandText = packet.Message.Substring(prefix.Length);
        
        var player = _pixelPlayerManager.GetPlayer(packet.PlayerId);
        if (player == null) return;
        
        var sender = CreateSender(player, prefix);
        
        OnPlayerCommand(prefix, sender, commandText);
    }

    private void OnDirectMessage(PlayerDirectMessagePacket packet)
    {
        var prefix = "//";
        if (!packet.Message.StartsWith(prefix)) return;
        
        var commandText = packet.Message.Substring(prefix.Length);
        
        var player = _pixelPlayerManager.GetPlayer(packet.FromPlayerId);
        if (player == null) return;
        
        var sender = CreateSender(player, prefix);
        
        OnPlayerCommand(prefix, sender, commandText);
    }

    public void OnPlayerCommand(string prefix, ICommandSender sender, string commandText)
    {
        var args = commandText.Split(" ");
        var command = ChatCommands.FirstOrDefault(cmd => cmd.CheckNameMatch(args[0]));
        
        if (command == null)
        {
            // Command not found.
            if (CommandMessages.UnknownCommand != null)
            {
                sender.SendMessage(CommandMessages.UnknownCommand.Replace("%prefix%", prefix));
            }

            if (SendHelp)
            {
                HelpFormatter.SendHelp(sender, GetAvailableCommands(sender));
            }
            return;
        }
        
        // Command found check permissions
        if(!command.CheckPermission(sender)) {
            sender.SendMessage(CommandMessages.NoPermission);
            return;
        }

        var result = command.ExecuteCommand(sender, commandText, args.Skip(1).ToArray());
        if (command.IsAsync)
        {
            Task.Run(async () =>
            {
                try
                {
                    await result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception was caught while executing the command {CommandText}", commandText);
                }
            });
        }
        else
        {
            try
            {
                result.Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was caught while executing the command {CommandText}", commandText);
            }
        }
    }

    protected virtual ICommandSender CreateSender(T player, string prefixUsed)
    {
        return new CommandSender(player, _client, prefixUsed);
    }

    public void AddCommand(ChatCommand command)
    {
        ChatCommands.Add(command);

        if (RegisterCustomCommands)
        {
            if (command.GetFullPermission() == null || RegisterRestrictedCustomCommands)
            {
                _client.SendChat($"/custom register {command.Name}", prefix: false);
            }
        }
    }

    public void AddHelpCommand()
    {
        AddCommand(new BasicHelpCommand(this));
    }
}