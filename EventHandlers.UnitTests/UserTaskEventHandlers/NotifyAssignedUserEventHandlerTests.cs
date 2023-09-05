using Bogus;
using DataModel;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using DataModel.Models.UserTasks;
using EventHandlers.UserTaskEventHandlers;
using Events.UserTaskEvents;
using NotificationService;
using NSubstitute;

namespace EventHandlers.UnitTests.UserTaskEventHandlers;

public class NotifyAssignedUserEventHandlerTests : BaseUnitTest
{
    [Fact]
    public async Task Handle_ShouldSendEmail_When_AssignedUserIsDifferentFromCreatedByUser()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.AssignedToUser, GetUser())
            .RuleFor(userTask => userTask.CreatedByUser, GetUser())
            .Generate();
        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.UserTasks.Add(userTask);
        db.SaveChanges();

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.Received(1).SendEmail(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );
    }

    [Fact]
    public async Task Handle_ShouldAddTitle_When_SendingEmail()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.AssignedToUser, GetUser())
            .RuleFor(userTask => userTask.CreatedByUser, GetUser())
            .Generate();

        var expectedTitle = $"New Task: {userTask.Title}";

        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.UserTasks.Add(userTask);
        db.SaveChanges();

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.Received(1).SendEmail(
            expectedTitle,
            Arg.Any<string>(),
            Arg.Any<string>()
        );
    }

    [Fact]
    public async Task Handle_ShouldAddMessage_When_SendingEmail()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.AssignedToUser, GetUser())
            .RuleFor(userTask => userTask.CreatedByUser, GetUser())
            .Generate();

        var expectedMessage = $"{userTask.CreatedByUser.FirstName} has assigned you the task '{userTask.Title}'";

        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.UserTasks.Add(userTask);
        db.SaveChanges();

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.Received(1).SendEmail(
            Arg.Any<string>(),
            expectedMessage,
            Arg.Any<string>()
        );
    }

    [Fact]
    public async Task Handle_ShouldSendToUserEmail_When_SendingEmail()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.AssignedToUser, GetUser())
            .RuleFor(userTask => userTask.CreatedByUser, GetUser())
            .Generate();

        var expectedEmail = userTask.AssignedToUser!.Email;

        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.UserTasks.Add(userTask);
        db.SaveChanges();

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.Received(1).SendEmail(
            Arg.Any<string>(),
            Arg.Any<string>(),
            expectedEmail
        );
    }

    [Fact]
    public async Task Handle_ShouldNotSendEmail_When_UserTaskHasNoAssignedUser()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.CreatedByUser, GetUser())
            .Generate();

        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.UserTasks.Add(userTask);
        db.SaveChanges();

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.DidNotReceive().SendEmail(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );
    }

    [Fact]
    public async Task Handle_ShouldNotSendEmail_When_AssignedUserIsTheSameAsCreatedUser()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var user = GetUser();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.AssignedToUser, user)
            .RuleFor(userTask => userTask.CreatedByUser, user)
            .Generate();

        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.UserTasks.Add(userTask);
        db.SaveChanges();

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.DidNotReceive().SendEmail(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );
    }

    [Fact]
    public async Task Handle_ShouldNotSendEmail_When_UserTaskDoesNotExist()
    {
        var notificationServiceMock = Substitute.For<INotificationService>();
        var user = GetUser();
        var userTask = new Faker<UserTask>()
            .RuleFor(userTask => userTask.Title, f => f.Lorem.Sentence())
            .RuleFor(userTask => userTask.Description, f => f.Lorem.Sentences())
            .RuleFor(userTask => userTask.AssignedToUser, user)
            .RuleFor(userTask => userTask.CreatedByUser, user)
            .Generate();

        var @event = new UserTaskCreatedEvent
        {
            UserTaskId = userTask.Id
        };

        using var db = new DatabaseContext(DbContextOptions);

        var handler = new NotifyAssignedUserEventHandler(db, notificationServiceMock);

        // Act
        await handler.Handle(@event, CancellationToken.None);

        // Assert
        await notificationServiceMock.DidNotReceive().SendEmail(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>()
        );
    }

    private User GetUser()
    {
        var result = new Faker<User>()
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.StatusEnum, UserStatusEnum.Active)
            .Generate();

        return result;
    }
}