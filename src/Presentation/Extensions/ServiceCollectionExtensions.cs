using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using Serilog.Extensions.Logging;

namespace Presentation.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddPresentation(
        this IServiceCollection serviceCollection, 
        WebApplicationBuilder builder,
        Logger logger)
    {
        var loggerFactory = new SerilogLoggerFactory(logger);
        
        var connectionString = builder.Configuration.GetSection("ConnectionStrings:MainConnectionString").Value;
        serviceCollection.AddDbContext<ApplicationDataContext>(q =>
        {
            q.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(loggerFactory);
        },
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton);

        serviceCollection.AddHttpContextAccessor();
        
        return serviceCollection;
    }
}