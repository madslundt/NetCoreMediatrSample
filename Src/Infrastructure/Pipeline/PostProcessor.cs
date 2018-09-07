using MediatR.Pipeline;
using System;
using System.Threading.Tasks;

namespace Src.Infrastructure.Pipeline
{
    public class PostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    {
        public async Task Process(TRequest request, TResponse response)
        {
            // Logic goes in here
        }
    }
}
