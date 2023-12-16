using Presentation.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Username", GetCurrentUserName())
    .CreateLogger();

await builder
    .AddServices(logger)
    .AddLogger(logger)
    .BuildApplication()
    .LoadDb()
    .CheckIfDatabaseIsUpdated()
    .AddSwagger()
    .AddMiddleware()
    .AddApplicationServices()
    .RunAppAsync();
return;


static string GetCurrentUserName()
{
    return "TestUser";
}