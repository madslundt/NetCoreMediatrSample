using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.UserTasks;
using Infrastructure.StronglyTypedIds;

namespace DataModel.Models.Users;

public record UserId(string Value) : StronglyTypedIdBaseEntity<UserId>("us_", Value);

public class User : BaseModel<UserId>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public UserStatusRef StatusRef { get; set; } = null!;
    public UserStatusEnum StatusEnum { get; set; } = UserStatusEnum.Active;

    public ICollection<UserTask> CreatedUserTasks { get; set; } = null!;
    public ICollection<UserTask> AssignedToUserTasks { get; set; } = null!;
}