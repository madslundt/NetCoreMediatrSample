using Microsoft.Extensions.DependencyInjection;

namespace Components;

public static class ComponentsExtensions
{
    public static IServiceCollection AddComponents(this IServiceCollection services)
    {
        return services;
    }
}