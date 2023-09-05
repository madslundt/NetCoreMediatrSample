using Infrastructure.BackgroundJob;
using MediatR;

namespace Infrastructure.CQRS.Events;

public sealed class EventBus : IEventBus
{
    private readonly IBackgroundJobBus? _backgroundJobBus;
    private readonly IMediator _mediator;

    public EventBus(IMediator mediator, IBackgroundJobBus? backgroundJobBus)
    {
        _mediator = mediator;
        _backgroundJobBus = backgroundJobBus;
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
        if (_backgroundJobBus is null)
        {
            PublishEvent(@event);
        }
        else
        {
            await _backgroundJobBus.Enqueue(() => PublishEvent(@event));
        }
    }

    public void PublishEvent(IEvent @event)
    {
        _mediator.Publish(@event);
    }
}