namespace Infrastructure.CQRS.Events;

public interface IEventBus
{
    Task Commit(params IEvent[] events);
}