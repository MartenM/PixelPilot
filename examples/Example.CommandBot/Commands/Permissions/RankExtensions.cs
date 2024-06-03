namespace Example.CommandBot.Commands;

public static class RankExtensions
{
    public static List<string> GetPermissions(this Rank rank)
    {
        List<string> perms = new List<string>()
        {
            "bot.broadcast"
        };

        switch (rank)
        {
            case Rank.Default:
                return perms;
            case Rank.Admin:
                perms.Add("bot.disconnect");
                break;
        }


        return perms;
    }
}