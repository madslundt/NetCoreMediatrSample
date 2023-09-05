using Microsoft.Extensions.DependencyInjection;
using NotificationService;

namespace EventHandlers;

public static class EventHandlersExtensions
{
    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService.NotificationService>();

        return services;
    }
}