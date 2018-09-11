using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Src.Features.User;
using Test.Common;
using Xunit;

namespace Test.Features.User
{
    public class DoesUserExistTest : TestBaseInMemoryDatabase
    {
        [Fact]
        public async Task ThrowValidationExceptionWhenUserIdIsEmpty()
        {
            var fixture = new Fixture();
            var query = new DoesUserExist.Query(Guid.Empty);

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsEmpty()
        {
            var fixture = new Fixture();
            var query = new DoesUserExist.Query(string.Empty);

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsNull()
        {
            var fixture = new Fixture();
            var query = new DoesUserExist.Query(null);

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ReturnFalseWhenUserDoesNotExistOnEmail()
        {
            var fixture = new Fixture();
            var query = new DoesUserExist.Query(fixture.Create<MailAddress>().Address);

            var result = await _mediator.Send(query);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReturnFalseWhenUserDoesNotExistOnUserId()
        {
            var fixture = new Fixture();
            var query = new DoesUserExist.Query(Guid.NewGuid());

            var result = await _mediator.Send(query);

            result.Should().BeFalse();
        }

        //[Fact]
        //public async Task ReturnTrueWhenUserExistsOnEmail()
        //{
        //    var fixture = new Fixture();
        //    var query = new DoesUserExist.Query(fixture.Create<MailAddress>().Address);

        //    var result = await _mediator.Send(query);

        //    result.Should().BeFalse();
        //}

        //[Fact]
        //public async Task ReturnFalseWhenUserExistsOnUserId()
        //{
        //    var fixture = new Fixture();
        //    var query = new DoesUserExist.Query(Guid.NewGuid());

        //    var result = await _mediator.Send(query);

        //    result.Should().BeFalse();
        //}
    }
}
