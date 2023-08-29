namespace Infrastructure.StronglyTypedIds;

public abstract record StronglyTypedIdBaseEntity
{
    public string Prefix { get; } = null!;
    public string Value { get; }

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
        var newId = (T) Activator.CreateInstance(typeof(T), Ulid.NewUlid().ToString())!;
        var id = $"{newId.Prefix}{newId.Value}";
        return (T) Activator.CreateInstance(typeof(T), id)!;
    }

    public string GetPlaceholder()
    {
        return $"{Prefix}{new string('x', Ulid.NewUlid().ToString().Length)}";
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