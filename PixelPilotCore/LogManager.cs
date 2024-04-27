using Microsoft.Extensions.Logging;

namespace PixelPilot;

public static class LogManager
{
    private static readonly ILoggerFactory Factory = LoggerFactory.Create(config =>
    {
        config.AddConsole();
    });

    public static ILogger GetLogger(string name)
    {
        return Factory.CreateLogger($"PixelPilot.{name}");
    }
}