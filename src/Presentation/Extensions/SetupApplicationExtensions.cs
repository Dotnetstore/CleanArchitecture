using Application;
using Application.Common.Behaviors;
using Application.Extensions;
using Asp.Versioning;
using Domain;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Extensions;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presentation.Swagger;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using ILogger = Serilog.ILogger;

namespace Presentation.Extensions;

internal static class SetupApplicationExtensions
{
    internal static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, ILogger logger)
    {
        builder
            .Services
            .AddApplication()
            .AddInfrastructure()
            .AddPresentation(builder, logger)
            .AddMediator()
            .AddVersioning()
            .AddSwaggerService()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddControllers();

        return builder;
    }

    internal static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.Logging.ClearProviders();
        
        builder.Logging.AddSerilog(logger);

        return builder;
    }

    internal static WebApplication LoadDb(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return app;
    }

    private static IServiceCollection AddMediator(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(q =>
        {
            q.RegisterServicesFromAssemblies(
                typeof(IDomainAssemblyMarker).Assembly,
                typeof(IApplicationAssemblyMarker).Assembly,
                typeof(IInfrastructureAssemblyMarker).Assembly);
            q.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return serviceCollection;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1.0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
            x.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddMvc().AddApiExplorer();
    
        return serviceCollection;
    }

    private static IServiceCollection AddSwaggerService(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
            .AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());
    
        return serviceCollection;
    }

    internal static WebApplication BuildApplication(this WebApplicationBuilder builder)
    {
        return builder.Build();
    }

    internal static WebApplication AddSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                foreach (var description in app.DescribeApiVersions())
                {
                    x.SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName);
                }
            });
        }

        return app;
    }

    internal static WebApplication CheckIfDatabaseIsUpdated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
            throw new Exception("Database is not fully migrated for MoviesContext.");

        return app;
    }

    internal static WebApplication AddMiddleware(this WebApplication app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();

        return app;
    }

    internal static WebApplication AddApplicationServices(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }

    internal static Task RunAppAsync(this WebApplication app)
    {
        return app.RunAsync(new CancellationTokenSource().Token);
    }
}