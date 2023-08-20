using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataModel.Libs.Extensions;

public static class StronglyTypedIdExtensions
{
    public static PropertyBuilder<TProperty> SupportStronglyTypedId<TProperty>(this PropertyBuilder<TProperty> property)
        where TProperty : StronglyTypedIdBaseEntity
    {
        return property.HasConversion(
            id => id.Value,
            value => (TProperty) Activator.CreateInstance(typeof(TProperty), value)!
        );
    }
}