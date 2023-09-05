namespace DataModel.Models;

public abstract class BaseModelEnum<TEnum> where TEnum : Enum
{
    public BaseModelEnum()
    {
    }

    public BaseModelEnum(TEnum baseModelEnum)
    {
        Id = baseModelEnum;
        Name = baseModelEnum.ToString();
    }

    public TEnum Id { get; }
    public string Name { get; }
}