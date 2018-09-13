using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Src.Features.User;
using Test.Common;
using Xunit;

namespace Test.Features.User
{
    public class GetUserTest : TestBase
    {
        [Fact]
        public async Task ThrowValidationExceptionWhenIdIsEmpty()
        {
            var query = _fixture.Build<GetUser.Query>()
                .With(x => x.Id, Guid.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenIdIsMissing()
        {
            var query = _fixture.Build<GetUser.Query>()
                .Without(x => x.Id)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task GetUserWhenProvidingAValidId()
        {
            var query = _fixture.Build<GetUser.Query>()
                .Create();

            _db.Users.Add(new DataModel.Models.User
            {
                Id = query.Id
            });
            _db.SaveChanges();

            var result = await _mediator.Send(query);

            result.Should().NotBeNull();
        }
    }
}
