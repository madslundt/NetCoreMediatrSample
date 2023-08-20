namespace Infrastructure.StronglyTypedIds;

public abstract record StronglyTypedIdBaseEntity
{
    private readonly string _prefix;
    public string Value { get; }

    public override string ToString() => Value;

    protected StronglyTypedIdBaseEntity(string prefix, string? value = null)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            throw new InvalidStronglyTypedIdException($"{nameof(prefix)} is not defined");
        }

        if (value == null)
        {
            throw new InvalidStronglyTypedIdException($"{nameof(value)} must have a value");
        }

        _prefix = prefix;
        Value = string.IsNullOrEmpty(value)
            ? $"{_prefix}{Ulid.NewUlid().ToString().ToLowerInvariant()}"
            : value.ToLowerInvariant();
    }

    public string GetPlaceholder()
    {
        return $"{_prefix}{new string('x', Ulid.NewUlid().ToString().Length)}";
    }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(_prefix) ||
            !Value.StartsWith(_prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        return Ulid.TryParse(Value.Substring(_prefix.Length), out _);
    }
};