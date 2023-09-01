using System.ComponentModel;
using System.Linq.Expressions;
using Hangfire;

namespace Infrastructure.BackgroundJob.Hangfire;

public class HangfireJobBus : IBackgroundJobBus
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireJobBus(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    [DisplayName("{0}")]
    public async Task Enqueue(Expression<Action> methodCall)
    {
        _backgroundJobClient.Enqueue(methodCall);
    }

    public async Task Schedule(Expression<Action> methodCall, TimeSpan timeSpan)
    {
        _backgroundJobClient.Schedule(methodCall, timeSpan);
    }
}