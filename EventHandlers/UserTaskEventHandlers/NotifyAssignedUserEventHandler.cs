using DataModel;
using DataModel.Models.UserTasks;
using Events.UserTaskEvents;
using Infrastructure.CQRS.Events;
using Microsoft.EntityFrameworkCore;

namespace EventHandlers.UserTaskEventHandlers;

public class NotifyAssignedUserEventHandler : IEventHandler<UserTaskCreatedEvent>
{
    private readonly DatabaseContext _db;

    public NotifyAssignedUserEventHandler(DatabaseContext db)
    {
        _db = db;
    }

    public async Task Handle(UserTaskCreatedEvent @event, CancellationToken cancellationToken)
    {
        var userTask = await GetTaskWithAssignedUser(@event.UserTaskId, cancellationToken);
        if (userTask?.AssignedToUserId is null)
        {
            return;
        }

        var isAuthorAssignedToUserTask = userTask.AssignedToUserId == userTask.CreatedByUserId;
        if (isAuthorAssignedToUserTask)
        {
            return;
        }

        // TODO: Notify assigned user
        Console.WriteLine(
            $"{userTask.CreatedByUser.FirstName} has assigned you the task '{userTask.Title}'");
    }

    private async Task<UserTask?> GetTaskWithAssignedUser(UserTaskId userTaskId, CancellationToken cancellationToken)
    {
        var result = await _db.UserTasks
            .Include(userTask => userTask.CreatedByUser)
            .Where(userTask => userTask.Id == userTaskId)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}