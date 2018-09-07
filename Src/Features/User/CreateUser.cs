using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace Src.Features.User
{
    public class CreateUser
    {
        public class Command : IRequest<Result>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class GetUserValidator : AbstractValidator<Command>
        {
            public GetUserValidator()
            {
                RuleFor(user => user.FirstName).NotEmpty();
                RuleFor(user => user.LastName).NotEmpty();
                RuleFor(user => user.Email).NotEmpty().EmailAddress();
            }
        }

        public class GetUserHandler : IRequestHandler<Command, Result>
        {
            public GetUserHandler()
            {

            }

            public Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    Id = Guid.NewGuid()
                };

                return Task.FromResult(result);
            }
        }
    }
}
