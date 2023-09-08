namespace Infrastructure.StronglyTypedIds;

public static class StronglyTypedIdHelper
{
    public static bool IsStronglyTypedId(Type? type)
    {
        if (type is null)
        {
            return false;
        }

        return type.BaseType is {IsGenericType: true} baseType &&
               baseType.GetGenericTypeDefinition() == typeof(StronglyTypedIdBaseEntity<>);
    }
}