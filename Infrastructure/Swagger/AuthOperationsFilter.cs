using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger;

public class AuthOperationsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var noAuthRequired = context.ApiDescription.CustomAttributes()
            .Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

        if (noAuthRequired)
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new List<string>()
                }
            }
        };
    }
}