using Microsoft.EntityFrameworkCore;

namespace DataModel.Models.Users;

public class UserContext
{
    public static void Build(ModelBuilder builder)
    {
        builder.Entity<User>(b =>
        {
            b.AddBaseModelContext<User, UserId>();
            b.Property(p => p.FirstName);
            b.Property(p => p.LastName);
            b.Property(p => p.Email);

            b.HasOne(user => user.StatusRef)
                .WithMany()
                .HasForeignKey(user => user.StatusEnum)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });
    }
}