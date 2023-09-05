using System.Net;
using Bogus;
using Components.UserComponents.Queries;
using DataModel.Models.Refs.UserStatusRefs;
using DataModel.Models.Users;
using FluentAssertions;
using Newtonsoft.Json;

namespace Api.IntegrationTests.UserController;

public class GetUserTests : BaseIntegrationTest
{
    public GetUserTests(IntegrationTestApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("Not valid id")]
    [InlineData("123456789")]
    [InlineData("us_123456789")]
    [InlineData("un_01h9h119dmpapchz8ap7x1f26b")]
    public async Task GetUser_Should_ReturnBadRequest_When_UserIdIsNotValid(string userId)
    {
        var expectedStatusCode = HttpStatusCode.BadRequest;

        var response = await Client.GetAsync($"/api/users/{userId}");
        var actualStatusCode = response.StatusCode;

        actualStatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_When_UserIsNotFound()
    {
        var expectedStatusCode = HttpStatusCode.NotFound;

        var userId = UserId.New().ToString();
        var response = await Client.GetAsync($"/api/users/{userId}");
        var actualStatusCode = response.StatusCode;

        actualStatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public async Task Handle_Should_ReturnOK_When_UserIsFound()
    {
        var expectedStatusCode = HttpStatusCode.OK;

        var user = new Faker<User>()
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.StatusEnum, UserStatusEnum.Active)
            .Generate();

        Db.Users.Add(user);
        Db.SaveChanges();

        var response = await Client.GetAsync($"/api/users/{user.Id}");
        var actualStatusCode = response.StatusCode;

        actualStatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserResult_When_UserIsFound()
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

        Db.Users.Add(user);
        Db.SaveChanges();

        var response = await Client.GetAsync($"/api/users/{user.Id}");
        var json = await response.Content.ReadAsStringAsync();
        var actualResult = JsonConvert.DeserializeObject<GetUser.Result>(json);

        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}