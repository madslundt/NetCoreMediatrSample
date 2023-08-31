using DataModel;
using DataModel.Models.Users;
using Events.UserEvents;
using FluentValidation;
using Infrastructure.CQRS.Commands;

namespace Components.UserComponents.Commands;

public class CreateUser
{
    public class Command : ICommand<Result>
    {
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
    }

    public class CreateUserValidator : AbstractValidator<Command>
    {
        public CreateUserValidator()
        {
            RuleFor(command => command.FirstName).NotEmpty();
            RuleFor(command => command.LastName).NotEmpty();
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
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            try
            {
                _db.Add(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            var @event = new UserCreatedEvent
            {
                UserId = user.Id
            };
            await _db.SaveChangesAndCommitAsync(cancellationToken, @event);

            var result = new Result
            {
                Id = user.Id.ToString()
            };

            return result;
        }
    }
}