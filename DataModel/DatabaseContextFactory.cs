using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace DataModel
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(ConnectionStringKeys.App);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("No connection string");
            }

            Console.WriteLine($"\nConnection string: {connectionString}");
            Console.WriteLine("\n Are you sure you want to apply to that database [y/n]?");

            var answer = Console.ReadKey().KeyChar.ToString();

            if (!string.Equals(answer, "Y", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Aborted..");
            }

            builder.UseSqlServer(connectionString);

            return new DatabaseContext(builder.Options);
        }
    }
}
