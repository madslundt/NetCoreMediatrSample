using Microsoft.EntityFrameworkCore;

namespace DataModel.Models
{
    public class RoleContext
    {
        public static void Build(ModelBuilder builder)
        {
            builder.Entity<Role>(b =>
            {

            });
        }
    }
}
