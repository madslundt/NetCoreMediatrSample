using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Infrastructure.BackgroundJob.Hangfire;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, string connectionString)
    {
        services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(connectionString);
                configuration.UseColouredConsoleLogProvider();

                var jsonSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                configuration.UseSerializerSettings(jsonSettings);
            }
        );

        services.AddSingleton<IBackgroundJobBus, HangfireJobBus>();

        return services;
    }

    public static IServiceCollection AddHangfireWorker(this IServiceCollection services)
    {
        services.AddHangfireServer();

        return services;
    }

    public static WebApplication UseHangfireUI(this WebApplication app, string path)
    {
        app.UseHangfireDashboard(path);

        return app;
    }
}