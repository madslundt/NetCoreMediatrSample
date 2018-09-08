using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Src.Infrastructure.Pipeline
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
            _logger.LogDebug($"Handling {typeof(TRequest).FullName}", request);
        }
    }
}
