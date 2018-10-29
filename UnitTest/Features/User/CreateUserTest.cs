using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using API.Features.User;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Hangfire.Common;
using Hangfire.States;
using Moq;
using UnitTest.Common;
using Xunit;

namespace UnitTest.Features.User
{
    public class CreateUserTest : TestBase
    {
        [Fact]
        public async Task ThrowValidationExceptionWhenFirstNameIsEmpty()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .With(x => x.FirstName, string.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenFirstNameIsMissing()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .Without(x => x.FirstName)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenLastNameIsEmpty()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .With(x => x.LastName, string.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenLastNameIsMissing()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .Without(x => x.LastName)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsEmpty()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .With(x => x.Email, string.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsMissing()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .Without(x => x.Email)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsNotValid()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command));
        }
    }
}
