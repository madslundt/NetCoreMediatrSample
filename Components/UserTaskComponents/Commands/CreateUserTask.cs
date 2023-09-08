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
        public UserId UserId { get; init; } = null!;
        public UserId? AssignedUserId { get; init; }
    }

    public class CreateUserTaskValidator : AbstractValidator<Command>
    {
        public CreateUserTaskValidator()
        {
            RuleFor(command => command.Title).NotEmpty();
            RuleFor(command => command.Description).NotEmpty();
            RuleFor(command => command.UserId).IdMustBeValid();
            RuleFor(command => command.AssignedUserId).OptionalIdMustBeValid();
        }
    }

    public class Result
    {
        public UserTaskId Id { get; init; } = null!;
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
                AssignedToUserId = request.AssignedUserId,
                CreatedByUserId = request.UserId
            };

            _db.Add(task);
            var @event = new UserTaskCreatedEvent
            {
                UserTaskId = task.Id
            };
            await _db.SaveChangesAndCommitAsync(cancellationToken, @event);

            var result = new Result
            {
                Id = task.Id
            };

            return result;
        }
    }
}