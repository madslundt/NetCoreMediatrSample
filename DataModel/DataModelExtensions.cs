using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace DataModel;

public static class DataModelExtensions
{
    public static IServiceCollection AddDataModel(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DatabaseContext>(
            options =>
            {
                // options.UseSqlServer(connectionString);
                options.UseInMemoryDatabase("MyDb");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

        return services;
    }
}