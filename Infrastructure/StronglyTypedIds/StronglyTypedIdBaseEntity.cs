using System.ComponentModel;

namespace Infrastructure.StronglyTypedIds;

public abstract record StronglyTypedIdBaseEntity
{
    protected string Prefix { get; init; } = null!;
    protected string Value { get; init; } = null!;
    
    public sealed override string ToString() => Value;
    
    public static string GetPattern<T>() where T : StronglyTypedIdBaseEntity<T>
    {
        var instance = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        return instance.Prefix + "[0-7][0-9A-HJKMNP-TV-Z]{25}".ToLowerInvariant();
    }

    public static string GetPlaceholder<T>() where T : StronglyTypedIdBaseEntity<T>
    {
        var instance = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        return $"{instance.Prefix}{Ulid.Empty}";
    }

    protected static bool TryParse<T>(string id, out T? result) where T : StronglyTypedIdBaseEntity<T>
    {
        result = null;

        if (string.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        var instance = (T) Activator.CreateInstance(typeof(T), id)!;
        result = instance;

        return true;
    }
}

[TypeConverter(typeof(StronglyTypedIdConverter))]
public abstract record StronglyTypedIdBaseEntity<T> : StronglyTypedIdBaseEntity where T : StronglyTypedIdBaseEntity<T>
{
    protected StronglyTypedIdBaseEntity(string prefix, string value)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new InvalidStronglyTypedIdException($"{nameof(prefix)} is not defined");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidStronglyTypedIdException($"{nameof(value)} is not defined");
        }

        Prefix = prefix;
        Value = value.ToLowerInvariant();
    }

    public static T New()
    {
        var instance = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        var id = $"{instance.Prefix}{instance.Value}";
        return (T) Activator.CreateInstance(typeof(T), id)!;
    }

    public static bool TryParse(string id, out T? result)
    {
        return TryParse<T>(id, out result);
    }

    public static string GetPlaceholder()
    {
        return GetPlaceholder<T>();
    }

    public bool IsValid()
    {
        if (!Value.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        return Ulid.TryParse(Value.Substring(Prefix.Length), out _);
    }
}