using MediatR;

namespace Infrastructure.CQRS.Events;

public sealed class EventBus : IEventBus
{
    private readonly IMediator _mediator;

    public EventBus(IMediator mediator)
    {
        _mediator = mediator ?? throw new Exception($"Missing dependency '{nameof(IMediator)}'");
        ;
    }

    public async Task Commit(params IEvent[] events)
    {
        foreach (var @event in events)
        {
            await Commit(@event);
        }
    }

    private async Task Commit(IEvent @event)
    {
        // TODO
        // if (_outboxListener != null)
        // {
        //     await _outboxListener.Commit(@event);
        // }
        // else if (_eventListener != null)
        // {
        //     await _eventListener.Publish(@event);
        // }
        // else
        // {
        //     throw new ArgumentNullException("No event listener found");
        // }

        await _mediator.Publish(@event);
    }
}