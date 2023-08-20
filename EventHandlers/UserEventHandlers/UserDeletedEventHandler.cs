using Events.UserEvents;
using Infrastructure.CQRS.Events;

namespace EventHandlers.UserEventHandlers;

public class UserDeletedEventHandler : IEventHandler<UserDeletedEvent>
{
    public UserDeletedEventHandler()
    {
    }

    public async Task Handle(UserDeletedEvent @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User with id '{@event.UserId}' deleted");
    }
}