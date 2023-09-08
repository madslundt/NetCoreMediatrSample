using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
            // c.CustomSchemaIds(type => type.FullName);
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
            options.OperationFilter<AuthOperationsFilter>();
            options.SchemaFilter<StronglyTypedIdSchemaFilter>();

            // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //     In = ParameterLocation.Header,
            //     Description = "Please enter a valid token",
            //     Name = "Authorization",
            //     Type = SecuritySchemeType.OAuth2,
            //     BearerFormat = "JWT",
            //     Scheme = "Bearer",
            //     Flows = new OpenApiOAuthFlows
            //     {
            //         Implicit = new OpenApiOAuthFlow
            //         {
            //             AuthorizationUrl = new Uri("/auth-server/connect/authorize", UriKind.Absolute),
            //             Scopes = new Dictionary<string, string>()
            //         }
            //     }
            // });
        });

        return services;
    }

    public static WebApplication UseSwaggerWithUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static IMvcBuilder AddSwaggerJsonOptions(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return builder;
    }
}