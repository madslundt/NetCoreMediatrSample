using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataModel;

public static class DataModelExtensions
{
    public static IServiceCollection AddDataModel(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DatabaseContext>(
            options => { options.UseSqlServer(connectionString); });

        return services;
    }
}