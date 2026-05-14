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
    private static ILoggerFactory _factory = CreateFactory(LogLevel);

    /// <summary>
    /// Get the current log level.
    /// rasdaq will not output any logs which are less than <c>LogLevel</c>.
    /// </summary>
    public static RasdaqLogLevel LogLevel { get; private set; } = RasdaqLogLevel.Info;

    /// <summary>
    /// Set the current log level for rasdaq.
    /// Will not emit logs for anything below the level set.
    /// Trace and Debug should not be used in production - these may contain rasdaq logs.
    /// </summary>
    /// <param name="newLogLevel"></param>
    public static void SetLogLevel(RasdaqLogLevel newLogLevel)
    {
        _factory.Dispose();

        LogLevel = newLogLevel;

        _factory = CreateFactory(newLogLevel);
    }

    /// <summary>
    /// Create Serilog instance. Used for writing logs to files.
    /// </summary>
    private static Logger CreateSerilog(RasdaqLogLevel level)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Is(ConvertToSerilogLevel(level))
            .WriteTo.File(
                $"rasdaq.log",
                outputTemplate: "{Timestamp:yyyy:MM:dd HH:mm:ss} [{Level:u4}] {SourceContext}: {Message}{NewLine}{Exception}"
            )
            .CreateLogger();
    }

    /// <summary>
    /// Create ILogger Factory. Used for creating factories that emit to console.
    /// </summary>
    private static ILoggerFactory CreateFactory(RasdaqLogLevel level)
    {
        Logger serilog = CreateSerilog(level);

        return LoggerFactory.Create(builder =>
            builder.SetMinimumLevel(ConvertToMsLogLevel(level))
                .AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "yyyy:MM:dd HH:mm:ss ";
                })
                .AddSerilog(serilog, dispose: true)
        );
    }

    private static LogEventLevel ConvertToSerilogLevel(RasdaqLogLevel level)
    {
        return level switch
        {
            RasdaqLogLevel.Trace => LogEventLevel.Verbose,
            RasdaqLogLevel.Debug => LogEventLevel.Debug,
            RasdaqLogLevel.Info => LogEventLevel.Information,
            RasdaqLogLevel.Warning => LogEventLevel.Warning,
            RasdaqLogLevel.Error => LogEventLevel.Error,
            RasdaqLogLevel.Critical => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }

    private static MSLogLevel ConvertToMsLogLevel(RasdaqLogLevel level)
    {
        return level switch
        {
            RasdaqLogLevel.Trace => MSLogLevel.Trace,
            RasdaqLogLevel.Debug => MSLogLevel.Debug,
            RasdaqLogLevel.Info => MSLogLevel.Information,
            RasdaqLogLevel.Warning => MSLogLevel.Warning,
            RasdaqLogLevel.Error => MSLogLevel.Error,
            RasdaqLogLevel.Critical => MSLogLevel.Critical,
            _ => MSLogLevel.Information
        };
    }

    private static ILogger GetLogger(string loggerPath)
    {
        string name = Path.GetFileNameWithoutExtension(loggerPath);
        return _factory.CreateLogger(name);
    }

    /// <summary>
    /// Log a piece of tracing to the console.
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
    /// Log a piece of debug to the console. 
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
    /// Log information to the console.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callerPath"></param>
    public static void Info(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogInformation(message);
    }

    /// <summary>
    /// Log a warning to the console.
    /// </summary>
    /// <param name="message"></param>
    public static void Warning(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogWarning(message);
    }

    /// <summary>
    /// Log an error message to the console.
    /// </summary>
    /// <param name="message"></param>
    public static void Error(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogError(message);
    }

    /// <summary>
    /// Log a critical message to the console.
    /// </summary>
    /// <param name="message"></param>
    public static void Critical(string message, [CallerFilePath] string callerPath = "")
    {
        GetLogger(callerPath).LogCritical(message);
    }

    /// <summary>
    /// Log an exception to the console, with an optional message.
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