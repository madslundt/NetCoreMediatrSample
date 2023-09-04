using Bogus;
using Components.UserComponents.Queries;
using DataModel;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using FluentAssertions;
using Infrastructure.ExceptionHandling.Exceptions;
using Infrastructure.StronglyTypedIds;
using ValidationException = FluentValidation.ValidationException;

namespace Components.UnitTests.UserComponents.Queries;

public class GetUserUnitTests : BaseUnitTest
{
    [Theory]
    [InlineData("Not valid id")]
    [InlineData("123456789")]
    [InlineData("us_123456789")]
    [InlineData(null)]
    public void GetUserValidator_Should_ThrowValidationError_When_IdIsNotValid(string userId)
    {
        var query = new GetUser.Query
        {
            UserId = userId
        };

        using var db = new DatabaseContext(DbContextOptions);
        var handler = new GetUser.Handler(db);
        var act = () => handler.Handle(query, CancellationToken.None);

        act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public void Handle_Should_ReturnNotFoundException_When_UserIsNotFound()
    {
        var query = new GetUser.Query
        {
            UserId = StronglyTypedIdBaseEntity.New<UserId>().ToString()
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
            .RuleFor(user => user.StatusEnum, UserStatusEnum.Active)
            .Generate();
        var expectedResult = new GetUser.Result
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        var query = new GetUser.Query
        {
            UserId = user.Id.ToString(),
        };

        using var db = new DatabaseContext(DbContextOptions);
        db.Users.Add(user);
        db.SaveChanges();

        var handler = new GetUser.Handler(db);
        var actualResult = await handler.Handle(query, CancellationToken.None);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}