using System.Linq.Expressions;

namespace Infrastructure.BackgroundJob;

public interface IBackgroundJobBus
{
    Task<string> Enqueue(Expression<Action> methodCall);
    Task<string> Schedule(Expression<Action> methodCall, TimeSpan timeSpan);
    Task<string> EnqueueAfter(string jobId, Expression<Action> methodCall);
}