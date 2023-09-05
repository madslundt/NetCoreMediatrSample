using Events.UserEvents;
using Infrastructure.CQRS.Events;

namespace EventHandlers.UserEventHandlers;

public class LogUserDeletedEventHandler : IEventHandler<UserDeletedEvent>
{
    public Task Handle(UserDeletedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User with id '{@event.UserId}' deleted");
        return Task.CompletedTask;
    }
}