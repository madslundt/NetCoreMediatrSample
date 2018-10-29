using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.User
{
    public class DoesUserExist
    {
        public class Query : IRequest<bool>
        {
            public Guid UserId { get; }
            public string Email { get; }

            public Query(Guid userId)
            {
                UserId = userId;
            }
            public Query(string email)
            {
                Email = email;
            }
        }

        public class GetUserValidator : AbstractValidator<Query>
        {
            public GetUserValidator()
            {
                RuleFor(user => user).NotNull();

                RuleFor(user => user.UserId).NotEmpty().Unless(user => !string.IsNullOrWhiteSpace(user.Email));

                RuleFor(user => user.Email).NotEmpty().EmailAddress().Unless(user => user.UserId != Guid.Empty);
            }
        }

        public class DoesUserExistHandler : IRequestHandler<Query, bool>
        {
            private readonly DatabaseContext _db;

            public DoesUserExistHandler(DatabaseContext db)
            {
                _db = db;
            }

            public async Task<bool> Handle(Query message, CancellationToken cancellationToken)
            {
                var query = from user in _db.Users
                            where (!string.IsNullOrWhiteSpace(message.Email) && message.Email == user.Email) || (message.UserId != Guid.Empty && message.UserId == user.Id)
                            select user.Id;

                var result = await query.AnyAsync();

                return result;
            }
        }
    }
}
