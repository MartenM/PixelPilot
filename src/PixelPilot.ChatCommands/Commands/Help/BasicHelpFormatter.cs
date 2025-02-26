using System.Text;

namespace PixelPilot.ChatCommands.Commands.Help;

public class BasicHelpFormatter : IHelpFormatter
{
    public void SendHelp(ICommandSender sender, List<ChatCommand> subCommands)
    {
        if (subCommands.First().HasParent())
        {
            subCommands.ForEach(cmd => sender.SendMessage($"{sender.PrefixUsed}{cmd.GetFullName()}"));
        }
        else
        {
            var sb = new StringBuilder();
            subCommands.ForEach(cmd => sb.Append($"{sender.PrefixUsed}{cmd.GetFullName()} "));
            sender.SendMessage(sb.ToString());
        }
    }
}