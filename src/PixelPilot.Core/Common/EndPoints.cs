namespace PixelPilot.Common;

public static class EndPoints
{
    public const string StagingUrl = "pw-staging.rnc.priddle.nl";
    public const string ProductionUrl = "pixelwalker.net";

    public static string ApiEndpoint
    {
        get
        {
            if (UseStaging)
            {
                return $"https://api.{StagingUrl}";
            }
            return $"https://api.{ProductionUrl}";
        }
    }

    public static string GameHttpEndpoint
    {
        get
        {
            if (UseStaging)
            {
                return $"https://server.{StagingUrl}";
            }
            return $"https://server.{ProductionUrl}";
        }
    }

    public static string GameWebsocketEndpoint
    {
        get
        {
            if (UseStaging)
            {
                return $"wss://server.{StagingUrl}";
            }

            return $"wss://server.{ProductionUrl}";
        }
    }

    public static bool UseStaging
    {
        get
        {
            var apiEnv = Environment.GetEnvironmentVariable("PIXELWALKER_ENDPOINT");
            if (apiEnv == null)
            {
                return false;
            }

            if (apiEnv.ToLower() == "staging")
            {
                return true;
            }

            return false;
        }
    }
}