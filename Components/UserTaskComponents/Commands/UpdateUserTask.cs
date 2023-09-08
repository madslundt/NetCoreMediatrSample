using DataModel;
using DataModel.Models.Refs.TaskStatusRefs;
using DataModel.Models.Users;
using DataModel.Models.UserTasks;
using Events.UserTaskEvents;
using FluentValidation;
using Infrastructure.CQRS.Commands;
using Infrastructure.ExceptionHandling.Exceptions;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Components.UserTaskComponents.Commands;

public class UpdateUserTask
{
    public class Command : ICommand
    {
        public UserTaskId UserTaskId { get; init; } = null!;
        public UserId AssignToUserId { get; init; } = null!;
    }

    public class AssignTaskToUserValidator : AbstractValidator<Command>
    {
        public AssignTaskToUserValidator()
        {
            RuleFor(command => command.UserTaskId).IdMustBeValid();
            RuleFor(command => command.AssignToUserId).IdMustBeValid();
        }
    }

    public class Handler : ICommandHandler<Command>
    {
        private readonly DatabaseContext _db;

        public Handler(DatabaseContext db)
        {
            _db = db;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var task = await GetTask(request.UserTaskId, cancellationToken);

            if (task is null)
            {
                throw new NotFoundException(nameof(task), request.UserTaskId);
            }

            task.AssignedToUserId = request.AssignToUserId;

            _db.Update(task);
            var @event = new UserTaskUpdatedEvent
            {
                UserTaskId = task.Id
            };
            await _db.SaveChangesAndCommitAsync(cancellationToken, @event);
        }

        private async Task<UserTask?> GetTask(UserTaskId userTaskId, CancellationToken cancellationToken)
        {
            var query = from task in _db.UserTasks
                where task.Id == userTaskId && task.StatusEnum != TaskStatusEnum.Removed
                select task;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}