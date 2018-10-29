using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.User
{
    public class GetUser
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Created { get; set; }
        }

        public class GetUserValidator : AbstractValidator<Query>
        {
            public GetUserValidator()
            {
                RuleFor(user => user.Id).NotEmpty();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<DataModel.Models.User, Result>(MemberList.Source);
            }
        }

        public class GetUserHandler : IRequestHandler<Query, Result>
        {
            private readonly DatabaseContext _db;
            private readonly IMapper _mapper;

            public GetUserHandler(DatabaseContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query message, CancellationToken cancellationToken)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == message.Id);

                if (user is null)
                {
                    throw new ArgumentNullException($"{nameof(user)} was not found");
                }

                var result = _mapper.Map<DataModel.Models.User, Result>(user);

                return result;
            }
        }
    }
}
