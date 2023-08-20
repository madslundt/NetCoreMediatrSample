using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddMicrosoftIdentityWebApi(options =>
        //         {
        //             configuration.Bind("AuthOptions", options);
        //
        //             options.TokenValidationParameters.NameClaimType = "name";
        //         },
        //         options => { configuration.Bind("AuthOptions", options); });

        return services;
    }

    public static WebApplication UseAuthenticationWithAuthorization(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static ControllerActionEndpointConventionBuilder ApplyAuthorization(
        this ControllerActionEndpointConventionBuilder builder, bool hasAuthorization = false)
    {
        if (hasAuthorization)
        {
            builder.RequireAuthorization();
        }

        return builder;
    }
}