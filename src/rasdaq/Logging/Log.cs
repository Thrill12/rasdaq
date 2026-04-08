using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace rasdaq.Logging;

public static class Log
{
    private static LogLevel _logLevel = LogLevel.Trace;
    private static ILoggerFactory _factory = CreateFactory(_logLevel);

    /// <summary>
    /// Gets the current log level.
    /// rasdaq will not output any logs which are less than LogLevel.
    /// </summary>
    public static LogLevel LogLevel => _logLevel;

    /// <summary>
    /// Sets the current log level for rasdaq.
    /// Will not emit logs for anything below the level set.
    /// Trace and Debug should not be used in production - these may contain rasdaq logs.
    /// </summary>
    /// <param name="newLogLevel"></param>
    public static void SetLogLevel(LogLevel newLogLevel)
    {
        _logLevel = newLogLevel;
        _factory = CreateFactory(_logLevel);
    }

    private static ILoggerFactory CreateFactory(LogLevel level)
    {
        return LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(level);
            builder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "yyyy:MM:dd HH:mm:ss ";
            });
        });
    }

    private static ILogger GetLogger(string loggerPath)
    {
        string name = Path.GetFileNameWithoutExtension(loggerPath);
        return _factory.CreateLogger(name);
    }

    /// <summary>
    /// Logs a piece of tracing to the console.
    /// rasdaq uses this to emit debugging information.
    /// Should not be emitted in production.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callerPath"></param>
    public static void Trace(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogTrace(message);
    }

    /// <summary>
    /// Logs a piece of debug to the console. 
    /// rasdaq uses this to emit debugging information.
    /// Should not be emitted in production.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callerPath"></param>
    public static void Debug(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogDebug(message);
    }

    /// <summary>
    /// Logs information to the console.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callerPath"></param>
    public static void Info(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogInformation(message);
    }

    /// <summary>
    /// Logs a warning to the console.
    /// </summary>
    /// <param name="message"></param>
    public static void Warning(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogWarning(message);
    }

    /// <summary>
    /// Logs an error message to the console.
    /// </summary>
    /// <param name="message"></param>
    public static void Error(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogError(message);
    }

    /// <summary>
    /// Logs a critical message to the console.
    /// </summary>
    /// <param name="message"></param>
    public static void Critical(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogCritical(message);
    }

    /// <summary>
    /// Logs an exception to the console, with an optional message.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="message"></param>
    public static void Exception(
        Exception ex,
        string message = "",
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "")
    {
        string label = string.IsNullOrEmpty(message) ? ex.Message : message;
        GetLogger(callerPath).LogError(ex, "{Member}: {Message}", callerMember, label);
    }
}