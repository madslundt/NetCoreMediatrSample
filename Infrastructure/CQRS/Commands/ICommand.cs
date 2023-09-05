using MediatR;

namespace Infrastructure.CQRS.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface ICommand : IRequest
{
}