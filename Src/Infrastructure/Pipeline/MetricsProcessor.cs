using App.Metrics;
using App.Metrics.Timer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Src.Infrastructure.Pipeline
{
    public class MetricsProcessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMetricsRoot _metrics;

        public MetricsProcessor(IMetricsRoot metrics)
        {
            _metrics = metrics;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestTimer = new TimerOptions
            {
                Name = "Mediator Timer",
                MeasurementUnit = App.Metrics.Unit.Requests,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };

            using (_metrics.Measure.Timer.Time(requestTimer))
            {
                var response = await next();

                return response;
            }
        }
    }
}
