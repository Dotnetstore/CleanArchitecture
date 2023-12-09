namespace Presentation.Extensions;

internal static class SetupApplicationExtensions
{
    internal static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
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
            app.UseSwaggerUI();
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