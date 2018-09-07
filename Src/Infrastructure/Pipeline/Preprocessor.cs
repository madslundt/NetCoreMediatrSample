using MediatR.Pipeline;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Src.Infrastructure.Pipeline
{
    public class Preprocessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            // Logic goes in here
        }
    }
}
