using System.Text;
using PixelPilot.ChatCommands.Commands.Help;

namespace PixelPilot.ChatCommands.Commands;

public abstract class ChatCommand : ICommandExecutor
{
    public string Name { get; }
    public string Description { get; }
    public string? Permission { get; }

    public List<string> Aliases { get; } = new();
    
    public IHelpFormatter? HelpFormatter { get; set; }
    
    protected string? FullPermission { get; set; }
    public ChatCommand? Parent { get; set; }

    public bool IsAsync { get; set; } = false;
    
    public ChatCommand(string name, string description, string? permission)
    {
        Name = name;
        Description = description;
        Permission = permission;
    }
    
    /// <summary>
    /// Checks if the input matches the name or aliases of this command.
    /// </summary>
    /// <param name="input">Input to test</param>
    /// <returns></returns>
    public bool CheckNameMatch(string input) {
        if(string.Equals(input, Name, StringComparison.OrdinalIgnoreCase)) return true;
        return Aliases.Any(alias => string.Equals(alias, input, StringComparison.OrdinalIgnoreCase));
    }
    
    /**
     * Permission check for this node.
     * @param sender The command sender
     * @return True if allowed
     */
    public virtual bool CheckPermission(ICommandSender sender) {
        if(GetFullPermission() == null) return true;
        return sender.HasPermission(GetFullPermission());
    }
    
    public string GetAlias()
    {
        return Aliases.Count > 0 ? Aliases[0] : Name;
    }
    
    /**
     * Used to create the full permission node for a command.
     * Commands can have their own specific permission but in order to make nesting
     * easier the + operator can be used to concat to the permission of the parent node.
     * @return A command node like "commands.debug.test.xxx"
     */
    public string? GetFullPermission() {
        if(FullPermission != null) return FullPermission;

        // If it does not start with the + operator we return this root permission.
        // For cases were an argument has no permission (null) we get the permission of it's parent.
        if(Permission == null) {
            if(Parent == null) return null;
            return Parent.GetFullPermission();
        }
        
        if(!Permission.StartsWith("+")) return Permission;

        // Check if we have a parent. If not, thrown an exception
        if(Parent == null) throw new Exception($"Cannot concat the permission {Permission} to it's parent because it has none!");

        // Recursion step:
        string? parentPermission = Parent.GetFullPermission();

        // Check if the parent permission is null. If so we cannot attach to it.
        // The other option would be to skip it but this can lead to confusing permission nodes.
        if(parentPermission == null) throw new Exception($"Cannot concat the permission {Permission} to it's parent because it parent ({Parent.GetFullName(CommandNameFormat.RootAlias)}) has no permission!");

        FullPermission = parentPermission + "." + Permission.Substring(1);
        return FullPermission;
    }

    public string GetFullName() => GetFullName(CommandNameFormat.NoAlias);

    public string GetFullName(CommandNameFormat format) {
        var builder = new StringBuilder();
        var parent = Parent;
        while(parent != null) {
            switch (format) {
                case CommandNameFormat.NoAlias:
                    builder.Insert(0, parent.Name + " ");
                    break;
                case CommandNameFormat.AllAlias:
                    builder.Insert(0, parent.GetAlias() + " ");
                    break;
                case CommandNameFormat.RootAlias:
                    // ROOT has no parent
                    if(!parent.HasParent()) builder.Insert(0, parent.GetAlias() + " ");
                    else builder.Insert(0, parent.Name + " ");
                    break;
            }

            parent = parent.Parent;
        }

        if(format == CommandNameFormat.AllAlias) builder.Append(GetAlias());
        else builder.Append(Name);

        return builder.ToString();
    }

    public bool HasParent()
    {
        return Parent != null;
    }
    
    protected IHelpFormatter GetHelpFormatter() {
        if(HelpFormatter != null) return HelpFormatter;
        if(Parent != null) return Parent.GetHelpFormatter();

        HelpFormatter = new BasicHelpFormatter();
        return HelpFormatter;
    }

    public abstract Task ExecuteCommand(ICommandSender sender, string fullCommand, string[] args);
}