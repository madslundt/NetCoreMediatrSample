using Hangfire;
using MediatR;

namespace API.Infrastructure.MessageQueue
{
    public static class MediatorExtension
    {
        public static void Enqueue(this IMediator mediator, IRequest request)
        {
            BackgroundJob.Enqueue<HangfireMediator>(m => m.SendCommand(request));
        }
    }

    public class HangfireMediator
    {
        private readonly IMediator _mediator;

        public HangfireMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void SendCommand(IRequest request)
        {
            _mediator.Send(request).Wait();
        }
    }
}
