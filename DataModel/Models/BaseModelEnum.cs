namespace DataModel.Models;

public abstract class BaseModelEnum<TEnum> where TEnum : Enum 
{
    public TEnum Id { get; }
    public string Name { get; }

    public BaseModelEnum()
    {}

    public BaseModelEnum(TEnum baseModelEnum)
    {
        Id = baseModelEnum;
        Name = baseModelEnum.ToString();
    }
}