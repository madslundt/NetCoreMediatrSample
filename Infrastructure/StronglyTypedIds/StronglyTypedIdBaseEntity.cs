namespace Infrastructure.StronglyTypedIds;

public abstract record StronglyTypedIdBaseEntity
{
    public string Prefix { get; } = null!;
    public string Value { get; }

    public sealed override string ToString() => Value;

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

    public static T New<T>() where T : StronglyTypedIdBaseEntity
    {
        var instance = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        var id = $"{instance.Prefix}{instance.Value}";
        return (T) Activator.CreateInstance(typeof(T), id)!;
    }

    public static string GetPlaceholder<T>() where T : StronglyTypedIdBaseEntity
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
};