using Microsoft.EntityFrameworkCore;

namespace DataModel.Models.Refs.UserStatusRefs;

public class UserStatusRefContext
{
    public static void Build(ModelBuilder builder)
    {
        builder.Entity<UserStatusRef>(b => { b.AddBaseModelEnumExtensions<UserStatusRef, UserStatusEnum>(); });
    }
}