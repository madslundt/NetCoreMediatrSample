using Events.UserEvents;
using Infrastructure.CQRS.Events;

namespace EventHandlers.UserEventHandlers;

public class LogUserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public LogUserCreatedEventHandler()
    {
    }

    public Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Created with id '{@event.UserId}' created");
        return Task.CompletedTask;
    }
}