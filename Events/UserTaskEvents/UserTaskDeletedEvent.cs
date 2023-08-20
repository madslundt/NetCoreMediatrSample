using DataModel.Models.UserTasks;
using Infrastructure.CQRS.Events;

namespace Events.UserTaskEvents;

public class UserTaskDeletedEvent : Event
{
    public UserTaskId UserTaskId { get; init; } = null!;
}