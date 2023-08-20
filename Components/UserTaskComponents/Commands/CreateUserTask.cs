using DataModel;
using DataModel.Models.Users;
using DataModel.Models.UserTasks;
using Events.UserTaskEvents;
using FluentValidation;
using Infrastructure.CQRS.Commands;
using Infrastructure.StronglyTypedIds;

namespace Components.UserTaskComponents.Commands;

public class CreateUserTask
{
    public class Command : ICommand<Result>
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string UserId { get; init; } = null!;
        public string? AssignedUserId { get; init; }
    }

    public class CreateTaskValidator : AbstractValidator<Command>
    {
        public CreateTaskValidator()
        {
            RuleFor(command => command.UserId).IdMustBeValid<UserId, Command>();
            RuleFor(command => command.AssignedUserId).OptionalIdMustBeValid<UserId, Command>();
        }
    }

    public class Result
    {
        public string Id { get; init; } = null!;
    }

    public class Handler : ICommandHandler<Command, Result>
    {
        private readonly DatabaseContext _db;

        public Handler(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var task = new UserTask
            {
                Title = request.Title,
                Description = request.Description,
                AssignedToUserId = request.AssignedUserId != null ? new UserId(request.AssignedUserId) : null,
                CreatedByUserId = new UserId(request.UserId),
            };

            _db.Add(task);
            var @event = new UserTaskCreatedEvent
            {
                UserTaskId = task.Id
            };
            await _db.SaveChangesAndCommitAsync(cancellationToken, @event);

            var result = new Result
            {
                Id = task.Id.Value
            };

            return result;
        }
    }
}