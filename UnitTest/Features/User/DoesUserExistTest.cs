using System;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Features.User;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using UnitTest.Common;
using Xunit;

namespace UnitTest.Features.User
{
    public class DoesUserExistTest : TestBase
    {
        [Fact]
        public async Task ThrowValidationExceptionWhenUserIdIsEmpty()
        {
            var query = new DoesUserExist.Query(Guid.Empty);

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsEmpty()
        {
            var query = new DoesUserExist.Query(string.Empty);

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsNull()
        {
            var query = new DoesUserExist.Query(null);

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(query));
        }

        [Fact]
        public async Task ReturnFalseWhenUserDoesNotExistOnEmail()
        {
            var query = new DoesUserExist.Query(_fixture.Create<MailAddress>().Address);

            var result = await _mediator.Send(query);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReturnFalseWhenUserDoesNotExistOnUserId()
        {
            var query = new DoesUserExist.Query(Guid.NewGuid());

            var result = await _mediator.Send(query);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReturnTrueWhenUserExistsOnEmail()
        {
            var user = _fixture.Build<DataModel.Models.User>()
                .With(x => x.Email, _fixture.Create<MailAddress>().Address)
                .Create();

            _db.Users.Add(user);
            _db.SaveChanges();

            var query = new DoesUserExist.Query(user.Email);

            var result = await _mediator.Send(query);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task ReturnFalseWhenUserExistsOnUserId()
        {
            var user = _fixture.Build<DataModel.Models.User>()
                .With(x => x.Email, _fixture.Create<MailAddress>().Address)
                .Create();

            _db.Users.Add(user);
            _db.SaveChanges();

            var query = new DoesUserExist.Query(user.Id);

            var result = await _mediator.Send(query);

            result.Should().BeTrue();
        }
    }
}
