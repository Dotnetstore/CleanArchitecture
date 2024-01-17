using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Presentation.Exceptions;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Presentation.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddPresentation(
        this IServiceCollection serviceCollection, 
        WebApplicationBuilder builder,
        ILogger logger)
    {
        var loggerFactory = new SerilogLoggerFactory(logger);
        
        var connectionString = builder.Configuration.GetSection("ConnectionStrings:MainConnectionString").Value;
        serviceCollection.AddDbContext<ApplicationDataContext>(q =>
            {
                q.UseSqlServer(connectionString, q =>
                    {
                        q.MaxBatchSize(50);
                        q.EnableRetryOnFailure();
                        q.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    })
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(loggerFactory);
            },
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton);

        serviceCollection.AddExceptionHandler<AppExceptionHandler>();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddHealthChecks();
        
        return serviceCollection;
    }
}