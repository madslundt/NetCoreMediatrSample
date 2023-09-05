using DataModel;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using FluentValidation;
using Infrastructure.CQRS.Queries;
using Infrastructure.ExceptionHandling.Exceptions;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Components.UserComponents.Queries;

public class GetUser
{
    public class Query : IQuery<Result>
    {
        public string UserId { get; init; } = null!;
    }

    public class GetUserValidator : AbstractValidator<Query>
    {
        public GetUserValidator()
        {
            RuleFor(query => query.UserId).IdMustBeValid<UserId, Query>();
        }
    }

    public class Result
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
    }

    public class Handler : IQueryHandler<Query, Result>
    {
        private readonly DatabaseContext _db;

        public Handler(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId);
            var result = await GetActiveUser(userId, cancellationToken);

            if (result is null)
            {
                throw new NotFoundException("User", userId.ToString());
            }

            return result;
        }

        private async Task<Result?> GetActiveUser(UserId userId, CancellationToken cancellationToken)
        {
            var query = from user in _db.Users
                where user.Id == userId && user.StatusEnum == UserStatusEnum.Active
                select new Result
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}