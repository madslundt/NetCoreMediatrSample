using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Pipeline
{
    public class PostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        private readonly ILogger<PostProcessor<TRequest, TResponse>> _logger;

        public PostProcessor(ILogger<PostProcessor<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task Process(TRequest request, TResponse response)
        {
            _logger.LogInformation($"Handled {typeof(TRequest).FullName}", new
            {
                Request = request,
                Response = response
            });
        }
    }
}
