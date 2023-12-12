using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class ApplicationDataContext : DbContext
{
    public ApplicationDataContext(DbContextOptions options) : base(options)
    {
        // if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator dbCreater)
        // {
        //     if (!dbCreater.CanConnect())
        //     {
        //         dbCreater.Create();
        //     }
        //
        //     if (!dbCreater.HasTables())
        //     {
        //         dbCreater.CreateTables();
        //     }
        // }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IDomainAssemblyMarker).Assembly);
    }
}