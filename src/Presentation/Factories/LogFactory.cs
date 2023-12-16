using Serilog;
using ILogger = Serilog.ILogger;

namespace Presentation.Factories;

internal static class LogFactory
{
    internal static ILogger CreateLogger(this WebApplicationBuilder builder)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Username", GetCurrentUserName())
            .CreateLogger();
    }

    static string GetCurrentUserName()
    {
        return "TestUser";
    }
}