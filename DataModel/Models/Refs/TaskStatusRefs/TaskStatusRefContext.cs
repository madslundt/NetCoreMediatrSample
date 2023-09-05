using Microsoft.EntityFrameworkCore;

namespace DataModel.Models.Refs.TaskStatusRefs;

public class TaskStatusRefContext
{
    public static void Build(ModelBuilder builder)
    {
        builder.Entity<TaskStatusRef>(b => { b.AddBaseModelEnumExtensions<TaskStatusRef, TaskStatusEnum>(); });
    }
}