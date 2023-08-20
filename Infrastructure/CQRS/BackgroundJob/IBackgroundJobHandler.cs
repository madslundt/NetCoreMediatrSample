using MediatR;

namespace Infrastructure.CQRS.BackgroundJob;

public interface IBackgroundJobHandler<in TRequest> : IRequestHandler<TRequest> where TRequest : IBackgroundJob
{ }