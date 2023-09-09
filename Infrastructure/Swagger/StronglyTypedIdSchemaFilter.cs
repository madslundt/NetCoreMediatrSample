using Infrastructure.StronglyTypedIds;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger;

public class StronglyTypedIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema? schema, SchemaFilterContext context)
    {
        if (schema is not null && context?.Type.IsAssignableTo(typeof(StronglyTypedIdBaseEntity)) == true)
        {
            var methodPlaceholder = typeof(StronglyTypedIdBaseEntity).GetMethod(nameof(StronglyTypedIdBaseEntity.GetPlaceholder));
            var genericPlaceholder = methodPlaceholder!.MakeGenericMethod(context.Type);
            var placeholder = genericPlaceholder.Invoke(this, null) as string;
            schema.Format = placeholder;
            
            var methodPattern = typeof(StronglyTypedIdBaseEntity).GetMethod(nameof(StronglyTypedIdBaseEntity.GetPattern));
            var genericPattern = methodPattern!.MakeGenericMethod(context.Type);
            var pattern = genericPattern.Invoke(this, null) as string;
            schema.Pattern = pattern;
            
            schema.Type = "string";
            schema.AdditionalPropertiesAllowed = false;
            schema.Properties.Clear();
        }
    }
}