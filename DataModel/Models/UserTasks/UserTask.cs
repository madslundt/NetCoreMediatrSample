using DataModel.Models.Refs.TaskStatusRefs;
using DataModel.Models.Users;
using Infrastructure.StronglyTypedIds;

namespace DataModel.Models.UserTasks;

public record UserTaskId(string Value) : StronglyTypedIdBaseEntity<UserTaskId>("ut_", Value);

public class UserTask : BaseModel<UserTaskId>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public TaskStatusRef StatusRef { get; set; } = null!;
    public TaskStatusEnum StatusEnum { get; set; } = TaskStatusEnum.Created;

    public User CreatedByUser { get; set; } = null!;
    public UserId CreatedByUserId { get; set; } = null!;

    public User? AssignedToUser { get; set; }
    public UserId? AssignedToUserId { get; set; }
}