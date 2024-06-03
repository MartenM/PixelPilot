using PixelPilot.ChatCommands.Commands.Help;
using PixelPilot.ChatCommands.Messages;

namespace PixelPilot.ChatCommands.Commands;

public class RootCommand : ChatCommand
{
    private List<ChatCommand> _subCommands = new ();
    
    public RootCommand(string name, string description, string? permission) : base(name, description, permission)
    {
        
    }

    public override Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args)
    {
        if (_subCommands.Count == 0)
        {
            throw new Exception("No sub-commands for the command: " + GetFullName());
        }
        
        // If no arguments are provided check if there are any possible sub-commands.
        // Send a help about these. If the available subCommands.size() == 0 that means the sender cannot execute any due to missing
        // permissions or them being playerOnly commands.
        if(args.Length == 0) {
            List<ChatCommand> subCommands = GetSubCommands(sender);

            // Check if the subCommands are possible
            if(subCommands.Count == 0) {
                sender.SendMessage(CommandMessages.NoPermission);
                return Task.CompletedTask;
            }
            
            GetHelpFormatter().SendHelp(sender, subCommands);
            return Task.CompletedTask;
        }

        var sc = _subCommands.FirstOrDefault(cmd => cmd.CheckNameMatch(args[0]));
        if(sc == null) {
            sender.SendMessage(CommandMessages.UnknownArgument);
            return Task.CompletedTask;
        }
        
        // Do the permission check for the child.
        if(!sc.CheckPermission(sender)) {
            sender.SendMessage(CommandMessages.NoPermission);
            return Task.CompletedTask;
        }

        return sc.ExecuteCommand(sender, fullCommand, args.Skip(1).ToArray());
    }
    
    public List<ChatCommand> GetSubCommands(ICommandSender sender)
    {
        return _subCommands.Where(cmd => cmd.CheckPermission(sender)).ToList();
    }

    public void AddCommand(ChatCommand command)
    {
        _subCommands.Add(command);
        command.Parent = this;
    }

    public override bool CheckPermission(ICommandSender sender)
    {
        if (GetSubCommands(sender).Count != 0) return true;
        return base.CheckPermission(sender);
    }
}