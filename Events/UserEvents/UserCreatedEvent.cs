using DataModel.Models.Users;
using Infrastructure.CQRS.Events;

namespace Events.UserEvents;

public class UserCreatedEvent : Event
{
    public UserId UserId { get; init; } = null!;
}