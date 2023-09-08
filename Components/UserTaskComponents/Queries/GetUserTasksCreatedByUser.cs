using DataModel;
using DataModel.Models.Refs.TaskStatusRefs;
using DataModel.Models.Users;
using FluentValidation;
using Infrastructure.CQRS.Queries;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Components.UserTaskComponents.Queries;

public class GetUserTasksCreatedByUser
{
    public class Query : IQuery<Result>
    {
        public string UserId { get; init; } = null!;
    }

    public class GetUserTasksCreatedByUserValidator : AbstractValidator<Query>
    {
        public GetUserTasksCreatedByUserValidator()
        {
            RuleFor(query => query.UserId).IdMustBeValid<UserId, Query>();
        }
    }

    public class Result
    {
        public ICollection<UserTaskResult> UserTasks { get; init; } = null!;

        public class UserTaskResult
        {
            public string Title { get; init; } = null!;
            public string Description { get; init; } = null!;
            public UserId? AssignedToUserId { get; init; }
        }
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
            var tasks = await GetAvailableTasksFromUser(userId, cancellationToken);

            var result = new Result
            {
                UserTasks = tasks
            };

            return result;
        }

        private async Task<ICollection<Result.UserTaskResult>> GetAvailableTasksFromUser(UserId userId,
            CancellationToken cancellationToken)
        {
            var query = from task in _db.UserTasks
                where task.CreatedByUserId == userId && task.StatusEnum != TaskStatusEnum.Removed
                select new Result.UserTaskResult
                {
                    Title = task.Title,
                    Description = task.Description,
                    AssignedToUserId = task.AssignedToUserId
                };

            var result = await query.ToListAsync(cancellationToken);

            return result;
        }
    }
}