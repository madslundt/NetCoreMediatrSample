using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.StronglyTypedIds;

public static class StronglyTypedIdsExtensions
{
    public static IMvcBuilder AddStronglyTypedIds(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new StronglyTypedIdJsonConverterFactory());
        });

        return builder;
    }
}