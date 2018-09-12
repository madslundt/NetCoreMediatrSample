using Hangfire;

namespace Src.Infrastructure.Hangfire
{
    public interface IHangfireWrapper
    {
        IBackgroundJobClient BackgroundJobClient { get; }
    }
}
