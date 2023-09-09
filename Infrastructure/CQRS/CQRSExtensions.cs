using FluentValidation;
using Infrastructure.CQRS.Commands;
using Infrastructure.CQRS.Events;
using Infrastructure.CQRS.Queries;
using Infrastructure.ExceptionHandling;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CQRS;

public static class CQRSExtensions
{
    public static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddMediatR(options => { options.RegisterServicesFromAssemblies(assemblies); });

        services.AddValidatorsFromAssemblies(assemblies);

        services.AddScoped<ICommandBus, CommandBus>();
        services.AddScoped<IQueryBus, QueryBus>();
        services.AddScoped<IEventBus, EventBus>();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static WebApplication UseCQRS(this WebApplication app)
    {
        return app;
    }
}