using DataModel;
using DataModel.Models.Refs.TaskStatusRefs;
using DataModel.Models.UserTasks;
using Events.UserTaskEvents;
using FluentValidation;
using Infrastructure.CQRS.Commands;
using Infrastructure.ExceptionHandling.Exceptions;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Components.UserTaskComponents.Commands;

public class DeleteUserTask
{
    public class Command : ICommand
    {
        public string UserTaskId { get; init; } = null!;
    }

    public class DeleteTaskValidator : AbstractValidator<Command>
    {
        public DeleteTaskValidator()
        {
            RuleFor(command => command.UserTaskId).IdMustBeValid<UserTaskId, Command>();
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
            var taskId = new UserTaskId(request.UserTaskId);
            var task = await GetTask(taskId, cancellationToken);

            if (task is null)
            {
                throw new NotFoundException(nameof(task), taskId.ToString());
            }

            _db.Remove(task);
            var @event = new UserTaskDeletedEvent
            {
                UserTaskId = taskId
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