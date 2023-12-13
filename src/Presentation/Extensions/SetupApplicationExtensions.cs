using Application;
using Application.Common.Behaviors;
using Application.Extensions;
using Asp.Versioning;
using Domain;
using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Presentation.Swagger;
using Serilog;
using Serilog.Core;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Extensions;

internal static class SetupApplicationExtensions
{
    internal static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, Logger logger)
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

    internal static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder, Logger logger)
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

    internal static WebApplication AddApplicationServices(this WebApplication app)
    {
        app.UseHttpsRedirection();

        return app;
    }

    internal static Task RunAppAsync(this WebApplication app)
    {
        return app.RunAsync(new CancellationTokenSource().Token);
    }
}