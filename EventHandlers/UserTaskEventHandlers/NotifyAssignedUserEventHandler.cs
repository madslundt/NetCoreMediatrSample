using DataModel;
using DataModel.Models.UserTasks;
using Events.UserTaskEvents;
using Infrastructure.CQRS.Events;
using Microsoft.EntityFrameworkCore;
using NotificationService;

namespace EventHandlers.UserTaskEventHandlers;

public class NotifyAssignedUserEventHandler : IEventHandler<UserTaskCreatedEvent>
{
    private readonly DatabaseContext _db;
    private readonly INotificationService _notificationService;

    public NotifyAssignedUserEventHandler(DatabaseContext db, INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }

    public async Task Handle(UserTaskCreatedEvent @event, CancellationToken cancellationToken)
    {
        var userTask = await GetTaskWithAssignedUser(@event.UserTaskId, cancellationToken);
        if (userTask?.AssignedToUser is null)
        {
            return;
        }

        var isAuthorAssignedToUserTask = userTask.AssignedToUserId == userTask.CreatedByUserId;
        if (isAuthorAssignedToUserTask)
        {
            return;
        }

        var title = $"New Task: {userTask.Title}";
        var message = $"{userTask.CreatedByUser.FirstName} has assigned you the task '{userTask.Title}'";
        await _notificationService.SendEmail(title, message, userTask.AssignedToUser.Email);
    }

    private async Task<UserTask?> GetTaskWithAssignedUser(UserTaskId userTaskId, CancellationToken cancellationToken)
    {
        var result = await _db.UserTasks
            .Include(userTask => userTask.CreatedByUser)
            .Include(userTask => userTask.AssignedToUser)
            .Where(userTask => userTask.Id == userTaskId)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}