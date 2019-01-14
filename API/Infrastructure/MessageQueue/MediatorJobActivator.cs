using System;
using Hangfire;
using MediatR;

namespace API.Infrastructure.MessageQueue
{
    public class MediatorJobActivator : JobActivator
    {
        private readonly IMediator _mediator;

        public MediatorJobActivator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override object ActivateJob(Type type)
        {
            return new HangfireMediator(_mediator);
        }
    }
}
