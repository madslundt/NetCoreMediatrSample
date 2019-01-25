using DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DataModel
{
    // Add migration
    // dotnet ef migrations add <MIGRATION NAME> -s ../API

    // Update database with latest migration(s)
    // dotnet ef database update -s ../API

    // Remove latest migration
    // dotnet ef migrations remove -s ../API

    // Revert the database to a migration
    // dotnet ef database update <MIGRATION NAME> -s ../API

    // Generate SQL script
    // dotnet ef migrations script -s ../API

    // -s ../Src is used to point to appsettings in Src application. 
    // Remember to set environment eg. 'setx ASPNETCORE_ENVIRONMENT Development'

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserStatusRef> UserStatuses { get; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            UserContext.Build(builder);
            RoleContext.Build(builder);
        }
    }
}
