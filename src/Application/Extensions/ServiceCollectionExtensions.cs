using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        // serviceCollection.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>(ServiceLifetime.Singleton);

        return serviceCollection;
    }
}