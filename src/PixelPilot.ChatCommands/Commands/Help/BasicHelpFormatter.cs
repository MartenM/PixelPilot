using System.Text;

namespace PixelPilot.ChatCommands.Commands.Help;

public class BasicHelpFormatter : IHelpFormatter
{
    public string Prefix = ".";
    
    public void SendHelp(CommandSender sender, List<ChatCommand> subCommands)
    {
        if (subCommands.First().HasParent())
        {
            subCommands.ForEach(cmd => sender.SendMessage($"{Prefix}{cmd.GetFullName()}"));
        }
        else
        {
            var sb = new StringBuilder();
            subCommands.ForEach(cmd => sb.Append($"{Prefix}{cmd.GetFullName()} "));
            sender.SendMessage(sb.ToString());
        }
    }
}