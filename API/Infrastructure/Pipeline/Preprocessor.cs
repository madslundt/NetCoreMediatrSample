using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Pipeline
{
    public class PreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<PreProcessor<TRequest>> _logger;

        public PreProcessor(ILogger<PreProcessor<TRequest>> logger)
        {
            _logger = logger;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).FullName}", new
            {
                Request = request
            });
        }
    }
}
