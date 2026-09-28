using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure.Data;

/// <summary>
/// Design-time factory for Entity Framework migrations
/// 
/// Why is this needed?
/// - API project has appsettings.json (with connection string)
/// - Infrastructure project has DbContext (but NO appsettings.json)
/// - During migrations: Infrastructure project can't find appsettings.json
/// - Solution: Use --startup-project TaskManager.Api
///   → EF Core sets working directory to API project
///   → Factory reads appsettings.json from API project directory
/// 
/// Migration command:
///   dotnet ef migrations add InitialCreate 
///     --project TaskManager.Infrastructure 
///     --startup-project TaskManager.Api
/// 
/// The --startup-project tells EF Core where to find appsettings.json!
/// </summary>
public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        // Read connection string from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new TaskDbContext(optionsBuilder.Options);
    }
}

