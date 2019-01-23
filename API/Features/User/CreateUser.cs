using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Features.User
{
    public class CreateUser
    {
        public class Command : IRequest<Result>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
        }

        public class CreateUserValidator : AbstractValidator<Command>
        {
            public CreateUserValidator()
            {
                RuleFor(user => user.FirstName).NotEmpty();
                RuleFor(user => user.LastName).NotEmpty();
                RuleFor(user => user.Email).NotEmpty().EmailAddress();
                RuleFor(user => user.Password).NotEmpty().MinimumLength(6)
                    .WithMessage($"{nameof(Command.Password)} needs to have a length of at least 6 characters.");
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, DataModel.Models.User>(MemberList.Source);
            }
        }

        public class CreateUserHandler : IRequestHandler<Command, Result>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public CreateUserHandler(
                IMapper mapper,
                IMediator mediator)
            {
                _mapper = mapper;
                _mediator = mediator;
            }

            public async Task<Result> Handle(Command message, CancellationToken cancellationToken)
            {
                var userExists = await _mediator.Send(new DoesUserExist.Query(message.Email), cancellationToken);

                if (userExists)
                {
                    throw new DuplicateNameException($"{nameof(message.Email)} already exists");
                }

                var user = _mapper.Map<Command, DataModel.Models.User>(message);

                var result = new Result
                {
                    Id = user.Id
                };

                return result;
            }
        }
    }
}
