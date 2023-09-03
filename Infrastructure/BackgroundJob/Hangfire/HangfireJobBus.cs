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
    public Task<string> Enqueue(Expression<Action> methodCall)
    {
        var jobId = _backgroundJobClient.Enqueue(methodCall);
        return Task.FromResult(jobId);
    }

    public Task<string> Schedule(Expression<Action> methodCall, TimeSpan timeSpan)
    {
        var jobId = _backgroundJobClient.Schedule(methodCall, timeSpan);
        return Task.FromResult(jobId);
    }

    public Task<string> EnqueueAfter(string jobId, Expression<Action> methodCall)
    {
        var newJobId = _backgroundJobClient.ContinueJobWith(jobId, methodCall);
        return Task.FromResult(newJobId);
    }

    public static Task AddRecurringJob(string jobId, Expression<Action> methodCall, string cron)
    {
        RecurringJob.AddOrUpdate(jobId, methodCall, cron);
        return Task.CompletedTask;
    }

    public static Task RemoveRecurringJob(string jobId)
    {
        RecurringJob.RemoveIfExists(jobId);
        return Task.CompletedTask;
    }

    public static Task StartRecurringJob(string jobId)
    {
        RecurringJob.TriggerJob(jobId);
        return Task.CompletedTask;
    }
}