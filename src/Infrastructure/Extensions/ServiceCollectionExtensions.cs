using Application.Common.Interfaces;
using Infrastructure.Contexts;
using Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<ApplicationDataContext>(q =>
        {
            q.UseSqlServer("Connectionstring");
        });
        
        serviceCollection
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return serviceCollection;
    }
}