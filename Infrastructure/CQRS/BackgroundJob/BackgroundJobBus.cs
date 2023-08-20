using MediatR;

namespace Infrastructure.CQRS.BackgroundJob;

public sealed class BackgroundJobBus : IBackgroundJobBus
{
    private readonly IMediator _mediator;

    public BackgroundJobBus(IMediator mediator)
    {
        _mediator = mediator ?? throw new Exception($"Missing dependency '{nameof(IMediator)}'");
    }

    public async Task Enqueue(IBackgroundJob job, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(job, cancellationToken);
    }

    public async Task Enqueue(IEnumerable<IBackgroundJob> jobs, CancellationToken cancellationToken = default)
    {
        foreach (var job in jobs)
        {
            await Enqueue(job, cancellationToken);
        }
    }

    public async Task Enqueue(params IBackgroundJob[] jobs)
    {
        foreach (var job in jobs)
        {
            await Enqueue(job);
        }
    }
}