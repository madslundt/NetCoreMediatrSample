namespace Infrastructure.StronglyTypedIds;

public abstract record StronglyTypedIdBaseEntity<T> where T : StronglyTypedIdBaseEntity<T>
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

    public string Prefix { get; } = null!;
    public string Value { get; }

    public sealed override string ToString()
    {
        return Value;
    }

    public static T New()
    {
        var instance = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        var id = $"{instance.Prefix}{instance.Value}";
        return (T) Activator.CreateInstance(typeof(T), id)!;
    }

    public static string GetPlaceholder()
    {
        var instance = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        return $"{instance.Prefix}{new string('x', instance.Value.Length)}";
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