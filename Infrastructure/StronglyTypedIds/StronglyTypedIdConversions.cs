using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.StronglyTypedIds;

public static class StronglyTypedIdConversions
{
    private static readonly ConcurrentDictionary<Type, ValueConverter> StronglyTypedIdConverters = new();

    public static void AddStronglyTypedIdConversions(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (StronglyTypedIdHelper.IsStronglyTypedId(property.ClrType))
                {
                    var converter = StronglyTypedIdConverters.GetOrAdd(
                        property.ClrType,
                        _ => CreateStronglyTypedIdConverter(property.ClrType)!);
                    property.SetValueConverter(converter);
                }
            }
        }
    }

    public static ValueConverter? CreateStronglyTypedIdConverter(Type stronglyTypedIdType)
    {
        // id => id.Value
        var toProviderFuncType = typeof(Func<,>)
            .MakeGenericType(stronglyTypedIdType, typeof(string));
        var stronglyTypedIdParam = Expression.Parameter(stronglyTypedIdType, "id");
        var toProviderExpression = Expression.Lambda(
            toProviderFuncType,
            Expression.Property(stronglyTypedIdParam, "Value"),
            stronglyTypedIdParam);

        // value => new <StronglyTypedIdBaseEntity<T>>(value)
        var fromProviderFuncType = typeof(Func<,>)
            .MakeGenericType(typeof(string), stronglyTypedIdType);
        var valueParam = Expression.Parameter(typeof(string), "value");
        var ctor = stronglyTypedIdType.GetConstructor(new[] {typeof(string)});
        var fromProviderExpression = Expression.Lambda(
            fromProviderFuncType,
            Expression.New(ctor!, valueParam),
            valueParam);

        var converterType = typeof(ValueConverter<,>)
            .MakeGenericType(stronglyTypedIdType, typeof(string));

        return (ValueConverter) Activator.CreateInstance(
            converterType,
            toProviderExpression,
            fromProviderExpression,
            null)!;
    }
}