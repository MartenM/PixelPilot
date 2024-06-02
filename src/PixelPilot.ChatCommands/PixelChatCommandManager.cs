using PixelPilot.ChatCommands.Commands;
using PixelPilot.ChatCommands.Commands.Help;
using PixelPilot.ChatCommands.Messages;
using PixelPilot.PixelGameClient;
using PixelPilot.PixelGameClient.Messages;
using PixelPilot.PixelGameClient.Messages.Received;
using PixelPilot.PixelGameClient.Players;

namespace PixelPilot.ChatCommands;

public class PixelChatCommandManager<T> : IChatCommandManager where T : IPixelPlayer
{
    private PixelPilotClient _client;
    private PixelPlayerManager<T> _pixelPlayerManager;

    public List<string> CommandPrefixes { get; set; } = new() {"!", "."};
    public List<ChatCommand> ChatCommands { get; set; } = new();

    public IHelpFormatter HelpFormatter { get; set; } = new BasicHelpFormatter();
    public List<ChatCommand> GetAvailableCommands(ICommandSender sender)
    {
        return ChatCommands.Where(cmd => cmd.CheckPermission(sender)).ToList();
    }

    public PixelChatCommandManager(PixelPilotClient client, PixelPlayerManager<T> pixelPlayerManager)
    {
        _client = client;
        _pixelPlayerManager = pixelPlayerManager;
        
        _client.OnPacketReceived += OnPlayerPacket;
    }

    public void OnPlayerPacket(object _, IPixelGamePacket packet)
    {
        var chatPacket = packet as PlayerChatPacket;
        if (chatPacket == null) return;

        if (!CommandPrefixes.Any(prefix => chatPacket.Message.StartsWith(prefix))) return;
        
        var commandText = chatPacket.Message.Substring(1);
        var player = _pixelPlayerManager.GetPlayer(chatPacket.PlayerId);
        if (player == null) return;

        var args = commandText.Split(" ");
        var command = ChatCommands.FirstOrDefault(cmd => cmd.CheckNameMatch(args[0]));
        var sender = CreateSender(player);
        
        if (command == null)
        {
            // Command not found. Don't do anything.
            return;
        }
        
        // Command found check permissions
        if(!command.CheckPermission(sender)) {
            sender.SendMessage(CommandMessages.NoPermission);
            return;
        }
        
        var result = command.ExecuteCommand(sender, commandText, args.Skip(1).ToArray());
        result.Wait();
    }

    protected virtual ICommandSender CreateSender(T player)
    {
        return new CommandSender(player, _client);
    }

    public void AddCommand(ChatCommand command)
    {
        ChatCommands.Add(command);
    }

    public void AddHelpCommand()
    {
        ChatCommands.Add(new BasicHelpCommand(this));
    }
}