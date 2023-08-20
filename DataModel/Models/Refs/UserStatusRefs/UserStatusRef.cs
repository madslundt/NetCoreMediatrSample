namespace DataModel.Models.Refs.UserStatusRefs;

public class UserStatusRef : BaseModelEnum<UserStatusEnum>
{
    public UserStatusRef()
    {
    }

    public UserStatusRef(UserStatusEnum baseModelEnum) : base(baseModelEnum)
    {
    }
}