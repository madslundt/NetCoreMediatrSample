using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;


namespace Src.Features.User
{
    public class GetUser
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Guid Id { get; }
            public string Email { get; }
            public string FirstName { get; }
            public string LastName { get; }
            public DateTime Created { get; set; }
        }

        public class GetUserValidator : AbstractValidator<Query>
        {
            public GetUserValidator()
            {
                RuleFor(user => user.Id).NotEmpty();
            }
        }

        public class GetUserHandler : IRequestHandler<Query, Result>
        {
            public GetUserHandler()
            {

            }

            public Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Result());
            }
        }
    }
}
