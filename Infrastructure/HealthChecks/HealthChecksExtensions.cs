using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.HealthChecks;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks();

        return services;
    }

    public static WebApplication UseApiHealthChecks(this WebApplication app, string path = "/health")
    {
        app.UseHealthChecks(path);
        return app;
    }
}