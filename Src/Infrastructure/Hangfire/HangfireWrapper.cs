using Hangfire;

namespace Src.Infrastructure.Hangfire
{
    public class HangfireWrapper : IHangfireWrapper
    {
        public IBackgroundJobClient BackgroundJobClient => new BackgroundJobClient(JobStorage.Current);
    }
}
