using DataModel;
using DataModel.Models.Refs.TaskStatusRefs;
using DataModel.Models.UserTasks;
using FluentValidation;
using Infrastructure.CQRS.Queries;
using Infrastructure.ExceptionHandling.Exceptions;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Components.UserTaskComponents.Queries;

public class GetUserTask
{
    public class Query : IQuery<Result>
    {
        public string UserTaskId { get; init; } = null!;
    }

    public class GetUserTaskValidator : AbstractValidator<Query>
    {
        public GetUserTaskValidator()
        {
            RuleFor(query => query.UserTaskId)!.IdMustBeValid<UserTaskId, Query>();
        }
    }

    public class Result
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string CreatedByUserId { get; init; } = null!;
        public string? AssignedToUserId { get; init; }
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
            var taskId = new UserTaskId(request.UserTaskId);
            var result = await GetAvailableTask(taskId, cancellationToken);

            if (result is null)
            {
                throw new NotFoundException("User", request.UserTaskId);
            }

            return result;
        }

        private async Task<Result?> GetAvailableTask(UserTaskId userTaskId, CancellationToken cancellationToken)
        {
            var query = from task in _db.UserTasks
                where task.Id == userTaskId && task.StatusEnum != TaskStatusEnum.Removed
                select new Result
                {
                    Title = task.Title,
                    Description = task.Description,
                    AssignedToUserId = task.AssignedToUserId.ToString(),
                    CreatedByUserId = task.CreatedByUserId.ToString(),
                };

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}