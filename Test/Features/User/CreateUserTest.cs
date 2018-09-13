using System.Net.Mail;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Src.Features.User;
using Xunit;
using Test.Common;
using System.Linq;
using Hangfire.Common;
using Moq;
using Hangfire.States;
using AutoMapper;

namespace Test.Features.User
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

        [Fact]
        public async Task CallCreateUserWhenProvidingValidData()
        {
            var command = _fixture.Build<CreateUser.Command>()
                .With(x => x.Email, _fixture.Create<MailAddress>().Address)
                .Create();

            var result = await _mediator.Send(command);

            result.Should().NotBeNull();

            _jobClientMock.Verify(x => 
                x.Create(It.Is<Job>(job => 
                    job.Method.Name == nameof(CreateUser.CreateUserHandler.CreateUser)), It.IsAny<EnqueuedState>()));
        }

        [Fact]
        public void CreateUserOutsideHandler()
        {
            var expectedUser = _fixture.Build<DataModel.Models.User>()
                .With(x => x.Email, _fixture.Create<MailAddress>().Address)
                .Create();

            var mapper = new Mock<IMapper>();
            var handler = new CreateUser.CreateUserHandler(_db, mapper.Object, _jobClientMock.Object, _mediator);

            handler.CreateUser(expectedUser);

            var actualUser = _db.Users.FirstOrDefault();

            actualUser.Should().BeEquivalentTo(expectedUser);
        }
    }
}
