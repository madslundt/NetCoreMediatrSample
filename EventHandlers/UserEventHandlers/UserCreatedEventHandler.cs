using Events.UserEvents;
using Infrastructure.CQRS.Events;

namespace EventHandlers.UserEventHandlers;

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public UserCreatedEventHandler()
    {
    }

    public async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Created with id '{@event.UserId}' created");
    }
}