using Bogus;
using Components.UserComponents.Queries;
using DataModel;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using FluentAssertions;
using FluentValidation.TestHelper;
using Infrastructure.ExceptionHandling.Exceptions;

namespace Components.UnitTests.UserComponents.Queries;

public class GetUserUnitTests : BaseUnitTest
{
    public static IEnumerable<object[]> GetUserIds(string id)
    {
        return new List<UserId[]>
        {
            new UserId[] {new(id)}
        };
    }

    [Theory]
    [MemberData(nameof(GetUserIds), "Not valid id")]
    [MemberData(nameof(GetUserIds), "123456789")]
    [MemberData(nameof(GetUserIds), "us_123456789")]
    [MemberData(nameof(GetUserIds), "un_01h9h119dmpapchz8ap7x1f26b")]
    public void GetUserValidator_Should_ThrowValidationError_When_UserIdIsNotValid(UserId userId)
    {
        var query = new GetUser.Query
        {
            UserId = userId
        };

        var validator = new GetUser.GetUserValidator();
        var result = validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.UserId);
    }

    [Fact]
    public void GetUserValidator_Should_NotThrowValidationError_When_UserIdIsValid()
    {
        var query = new GetUser.Query
        {
            UserId = UserId.New()
        };

        var validator = new GetUser.GetUserValidator();
        var result = validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(q => q.UserId);
    }

    [Fact]
    public void Handle_Should_ReturnNotFoundException_When_UserIsNotFound()
    {
        var query = new GetUser.Query
        {
            UserId = UserId.New()
        };

        using var db = new DatabaseContext(DbContextOptions);
        var handler = new GetUser.Handler(db);
        var act = () => handler.Handle(query, CancellationToken.None);

        act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnGetUserResult_When_UserIsFound()
    {
        var user = new Faker<User>()
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.StatusEnum, UserStatusEnum.Active)
            .Generate();
        var expectedResult = new GetUser.Result
        {
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        var query = new GetUser.Query
        {
            UserId = user.Id
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.Users.Add(user);
        db.SaveChanges();

        var handler = new GetUser.Handler(db);
        var actualResult = await handler.Handle(query, CancellationToken.None);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}