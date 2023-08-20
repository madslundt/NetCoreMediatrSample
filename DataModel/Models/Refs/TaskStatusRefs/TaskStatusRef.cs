namespace DataModel.Models.Refs.TaskStatusRefs;

public class TaskStatusRef : BaseModelEnum<TaskStatusEnum>
{
    public TaskStatusRef()
    {
    }

    public TaskStatusRef(TaskStatusEnum baseModelEnum) : base(baseModelEnum)
    {
    }
}