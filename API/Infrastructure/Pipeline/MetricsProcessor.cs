using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Timer;
using MediatR;

namespace API.Infrastructure.Pipeline
{
    public class MetricsProcessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMetricsRoot _metrics;

        public MetricsProcessor(IMetricsRoot metrics)
        {
            _metrics = metrics;
        }
        public async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            var requestTimer = new TimerOptions
            {
                Name = "Mediator Pipeline",
                MeasurementUnit = App.Metrics.Unit.Requests,
                DurationUnit = TimeUnit.Milliseconds,
                RateUnit = TimeUnit.Milliseconds
            };

            TResponse response;
            using (_metrics.Measure.Timer.Time(requestTimer))
            {
                response = await next();
            }

            _metrics.ReportRunner.RunAllAsync();

            return response;
        }
    }
}
