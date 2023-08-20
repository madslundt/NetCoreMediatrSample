using DataModel.Models.Users;
using Infrastructure.CQRS.Events;

namespace Events.UserEvents;

public class UserDeletedEvent : Event
{
    public UserId UserId { get; init; } = null!;
}