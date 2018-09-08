using DataModel.Models.User;
using Microsoft.EntityFrameworkCore;

namespace DataModel
{
    // Add migration
    // dotnet ef migrations add <MIGRATION NAME> -s ../Src

    // Update database with latest migration(s)
    // dotnet ef database update -s ../Src

    // Remove latest migration
    // dotnet ef migrations remove -s ../Src

    // Revert a migration from database
    // dotnet ef database update <MIGRATION NAME> -s ../Src

    // Generate SQL script
    // dotnet ef migrations script -s ../Src

    // -s ../Src is the point to appsettings in Src

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserStatusRef> UserStatuses { get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            UserContext.Build(builder);
        }
    }
}
