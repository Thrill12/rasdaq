using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Runtime.CompilerServices;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using MSLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace rasdaq.Logging;

public static class Log
{
    private static RasdaqLogLevel _logLevel = RasdaqLogLevel.Info;
    private static ILoggerFactory _factory = CreateFactory(_logLevel);

    /// <summary>
    /// Gets the current log level.
    /// rasdaq will not output any logs which are less than LogLevel.
    /// </summary>
    public static RasdaqLogLevel LogLevel => _logLevel;

    /// <summary>
    /// Sets the current log level for rasdaq.
    /// Will not emit logs for anything below the level set.
    /// Trace and Debug should not be used in production - these may contain rasdaq logs.
    /// </summary>
    /// <param name="newLogLevel"></param>
    public static void SetLogLevel(RasdaqLogLevel newLogLevel)
    {
        _factory = CreateFactory(_logLevel);
    }

    /// <summary>
    /// Creates Serilog instance. Used for writing logs to files.
    /// </summary>
    private static Logger CreateSerilog(RasdaqLogLevel level)
    {
        Directory.CreateDirectory("logs");

        return new LoggerConfiguration()
            .MinimumLevel.Is(ConvertToSerilogLevel(level))
            .WriteTo.File(
                $"rasdaq.log",
                outputTemplate: "{Timestamp:yyyy:MM:dd HH:mm:ss} [{Level:u4}] {SourceContext}: {Message}{NewLine}{Exception}"
            )
            .CreateLogger();
    }

    /// <summary>
    /// Creates ILogger Factory. Used for creating factories that emit to console.
    /// </summary>
    private static ILoggerFactory CreateFactory(RasdaqLogLevel level)
    {
        Logger serilog = CreateSerilog(level);

        return LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(ConvertToMsLogLevel(level));
            builder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.TimestampFormat = "yyyy:MM:dd HH:mm:ss ";
            });
            builder.AddSerilog(serilog, dispose: true);
        });
    }

    private static LogEventLevel ConvertToSerilogLevel(RasdaqLogLevel level) => level switch
    {
        RasdaqLogLevel.Trace => LogEventLevel.Verbose,
        RasdaqLogLevel.Debug => LogEventLevel.Debug,
        RasdaqLogLevel.Info => LogEventLevel.Information,
        RasdaqLogLevel.Warning => LogEventLevel.Warning,
        RasdaqLogLevel.Error => LogEventLevel.Error,
        RasdaqLogLevel.Critical => LogEventLevel.Fatal,
        _ => LogEventLevel.Information
    };

    private static MSLogLevel ConvertToMsLogLevel(RasdaqLogLevel level) => level switch
    {
        RasdaqLogLevel.Trace => MSLogLevel.Trace,
        RasdaqLogLevel.Debug => MSLogLevel.Debug,
        RasdaqLogLevel.Info => MSLogLevel.Information,
        RasdaqLogLevel.Warning => MSLogLevel.Warning,
        RasdaqLogLevel.Error => MSLogLevel.Error,
        RasdaqLogLevel.Critical => MSLogLevel.Critical,
        _ => MSLogLevel.Information
    };

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

public enum RasdaqLogLevel
{
    Trace = 0,
    Debug = 1,
    Info = 2,
    Warning = 3,
    Error = 4,
    Critical = 5,
    None = 6
}