using Hangfire.Common;
using Hangfire.MediatR;
using MediatR;
using Newtonsoft.Json;

namespace Hangfire
{
    public static class HangfireExtensions
    {
        public static IGlobalConfiguration UseMediatR(this IGlobalConfiguration config, IMediator mediator)
        {
            config.UseActivator(new MediatRJobActivator(mediator));

            JobHelper.SetSerializerSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });

            return config;
        }
    }
}
