using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Src;
using Src.Features.User;
using Src.Infrastructure.Pipeline;
using Test.Common;
using Xunit;

namespace Test.Features.User
{
    public class GetUserTest : TestBase
    {
        [Fact]
        public async Task ThrowValidationExceptionWhenIdIsEmpty()
        {
            var fixture = new Fixture();
            var getUser = fixture.Build<GetUser.Query>()
                .With(x => x.Id, Guid.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(getUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenIdIsMissing()
        {
            var fixture = new Fixture();
            var getUser = fixture.Build<GetUser.Query>()
                .Without(x => x.Id)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(getUser));
        }

        [Fact]
        public async Task GetUserWhenProvidingAValidId()
        {
            var fixture = new Fixture();
            var getUser = fixture.Build<GetUser.Query>()
                .Create();

            _db.Users.Add(new DataModel.Models.User
            {
                Id = getUser.Id
            });
            _db.SaveChanges();

            var result = await _mediator.Send(getUser);

            result.Should().NotBeNull();
        }
    }
}
