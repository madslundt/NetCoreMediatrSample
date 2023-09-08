namespace DataModel.Models;

public abstract class BaseModelEnum<TEnum> where TEnum : Enum
{
    protected BaseModelEnum()
    {
    }

    protected BaseModelEnum(TEnum baseModelEnum)
    {
        Id = baseModelEnum;
        Name = baseModelEnum.ToString();
    }

    public TEnum Id { get; } = default!;
    public string Name { get; } = null!;
}