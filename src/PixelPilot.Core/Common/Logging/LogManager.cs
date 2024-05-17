using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PixelPilot.Common.Logging;

/// <summary>
/// The LogManager class provides static access to loggers though the project.
/// </summary>
public static class LogManager
{
    private static ILoggerFactory? _factory;
    private static Action<ILoggingBuilder>? _configureLogger;
    private static bool _loggerCreated;
    
    private static void _configure()
    {
        _factory =  LoggerFactory.Create(config =>
        {
            config.AddConsole();
            if (_configureLogger != null)
            {
                _configureLogger(config);
            }
        });
    }

    /// <summary>
    /// Set an action to be used when creating the logger factory.
    /// </summary>
    /// <param name="configure">Action to be used to configure</param>
    public static void Configure(Action<ILoggingBuilder> configure)
    {
        if (_loggerCreated) Console.WriteLine("Warn: Attempting to configure logger factory, but a logger was already created!");
        _configureLogger = configure;
    }

    /// <summary>
    /// Use a configuration section to configure the logger.
    /// </summary>
    /// <param name="section">Logging configuration</param>
    public static void Configure(IConfigurationSection section)
    {
        if (_loggerCreated) Console.WriteLine("Warn: Attempting to configure logger factory, but a logger was already created!");
        Configure(config =>
        {
            config.AddConfiguration(section);
        });
    }

    public static ILogger GetLogger(string name)
    {
        if (_factory == null)
        {
            _configure();
        }

        _loggerCreated = true;
        return _factory!.CreateLogger($"PixelPilot.{name}");
    }
}