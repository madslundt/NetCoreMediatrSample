using DataModel.Models.Refs.TaskStatusRefs;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using DataModel.Models.UserTasks;
using Infrastructure.CQRS.Events;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace DataModel;

public class DatabaseContext : DbContext
{
    private readonly IEventBus? _eventBus;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, IEventBus? eventBus = null) : base(options)
    {
        _eventBus = eventBus;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        UserContext.Build(builder);
        UserTaskContext.Build(builder);

        // Refs
        UserStatusRefContext.Build(builder);
        TaskStatusRefContext.Build(builder);

        // Add strongly typed id ef core conversions
        builder.AddStronglyTypedIdConversions();
    }

    public async Task<int> SaveChangesAndCommitAsync(CancellationToken cancellationToken = default,
        params IEvent[] events)
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        var result = await SaveChangesAsync(cancellationToken);
        if (_eventBus is not null)
        {
            await _eventBus.Commit(events);
        }

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}