using System.Linq.Expressions;

namespace Infrastructure.BackgroundJob;

public interface IBackgroundJobBus
{
    Task Enqueue(Expression<Action> methodCall);
    Task Schedule(Expression<Action> methodCall, TimeSpan timeSpan);
}