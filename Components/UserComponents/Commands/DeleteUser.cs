using DataModel;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using Events.UserEvents;
using FluentValidation;
using Infrastructure.CQRS.Commands;
using Infrastructure.ExceptionHandling.Exceptions;
using Infrastructure.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Components.UserComponents.Commands;

public class DeleteUser
{
    public class Command : ICommand
    {
        public string UserId { get; init; } = null!;
    }

    public class DeleteUserValidator : AbstractValidator<Command>
    {
        public DeleteUserValidator()
        {
            RuleFor(command => command.UserId).IdMustBeValid<UserId, Command>();
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
            var userId = new UserId(request.UserId);
            var user = await GetUser(userId, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException(nameof(user), userId.ToString());
            }

            _db.Remove(user);
            var @event = new UserDeletedEvent
            {
                UserId = user.Id
            };

            await _db.SaveChangesAndCommitAsync(cancellationToken, @event);
        }

        private async Task<User?> GetUser(UserId userId, CancellationToken cancellationToken)
        {
            var query = from user in _db.Users
                where user.Id == userId && user.StatusEnum == UserStatusEnum.Active
                select user;

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}