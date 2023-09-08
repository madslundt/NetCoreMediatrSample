using Microsoft.EntityFrameworkCore;

namespace DataModel.Models.UserTasks;

public class UserTaskContext
{
    public static void Build(ModelBuilder builder)
    {
        builder.Entity<UserTask>(b =>
        {
            b.AddBaseModelContext<UserTask, UserTaskId>();

            b.Property(p => p.Title);
            b.Property(p => p.Description);

            b.HasOne(task => task.StatusRef)
                .WithMany()
                .HasForeignKey(task => task.StatusEnum)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.HasOne(task => task.CreatedByUser)
                .WithMany(user => user.CreatedUserTasks)
                .HasForeignKey(task => task.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.HasOne(task => task.AssignedToUser)
                .WithMany(user => user.AssignedToUserTasks)
                .HasForeignKey(task => task.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}