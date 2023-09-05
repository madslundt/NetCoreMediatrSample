using MediatR;

namespace Infrastructure.CQRS.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}