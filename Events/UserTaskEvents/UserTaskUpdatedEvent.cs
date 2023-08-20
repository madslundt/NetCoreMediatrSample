using DataModel.Models.UserTasks;
using Infrastructure.CQRS.Events;

namespace Events.UserTaskEvents;

public class UserTaskUpdatedEvent : Event
{
    public UserTaskId UserTaskId { get; init; } = null!;
}