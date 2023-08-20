using Microsoft.Extensions.DependencyInjection;

namespace EventHandlers;

public static class EventHandlersExtensions
{
    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        return services;
    }
}