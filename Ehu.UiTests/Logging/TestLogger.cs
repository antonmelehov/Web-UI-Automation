using Serilog;

namespace Ehu.UiTests.Logging;

public static class TestLogger
{
    private static bool _isConfigured;

    public static void Configure()
    {
        if (_isConfigured)
            return;

        var logsDirectory = Path.Combine(AppContext.BaseDirectory, "TestResults", "Logs");
        Directory.CreateDirectory(logsDirectory);

        var logFilePath = Path.Combine(logsDirectory, "ui-tests-.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: logFilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                shared: true,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        _isConfigured = true;
    }

    public static void CloseAndFlush()
    {
        Log.CloseAndFlush();
        _isConfigured = false;
    }
}