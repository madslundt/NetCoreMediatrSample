using Hangfire;
using Hangfire.Common;
using MediatR;
using Newtonsoft.Json;

namespace API.Infrastructure.MessageQueue
{
    public static class HangfireExtensions
    {
        public static IGlobalConfiguration UseMediatR(this IGlobalConfiguration config, IMediator mediator)
        {
            config.UseActivator(new MediatorJobActivator(mediator));

            JobHelper.SetSerializerSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });

            return config;
        }
    }
}
