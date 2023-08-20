namespace Infrastructure.CQRS.BackgroundJob;

public interface IBackgroundJobBus
{

    Task Enqueue(IBackgroundJob job, CancellationToken cancellationToken = default);
    Task Enqueue(IEnumerable<IBackgroundJob> jobs, CancellationToken cancellationToken = default);
    Task Enqueue(params IBackgroundJob[] jobs);
}