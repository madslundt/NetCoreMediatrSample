using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cors;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string name, string origin = "*",
        string methods = "*", string headers = "*")
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name,
                b =>
                {
                    b.WithOrigins(origin);
                    b.WithMethods(methods);
                    b.WithHeaders(headers);
                });
        });

        return services;
    }

    public static WebApplication UseCorsPolicy(this WebApplication app, string name)
    {
        app.UseCors(name);

        return app;
    }
}