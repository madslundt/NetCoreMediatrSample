using MediatR;

namespace Infrastructure.CQRS.Events;

public interface IEventHandler<T> : INotificationHandler<T> where T : IEvent
{
}