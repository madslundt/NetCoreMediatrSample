using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataModel;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Directory.GetCurrentDirectory() + "/../Api/appsettings.json")
            .AddJsonFile(Directory.GetCurrentDirectory() + $"/../Api/appsettings.{environment}.json", true)
            .Build();
        var builder = new DbContextOptionsBuilder<DatabaseContext>();

        var connectionString = configuration.GetConnectionString(DataModelOptions.ConnectionString) ??
                               throw new Exception(
                                   $"{DataModelOptions.ConnectionString} is not found in configuration");

        builder.UseSqlServer(connectionString);

        return new DatabaseContext(builder.Options);
    }
}