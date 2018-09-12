using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Src;
using Src.Features.User;
using Src.Infrastructure.Pipeline;
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
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .With(x => x.FirstName, string.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenFirstNameIsMissing()
        {
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .Without(x => x.FirstName)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenLastNameIsEmpty()
        {
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .With(x => x.LastName, string.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenLastNameIsMissing()
        {
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .Without(x => x.LastName)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsEmpty()
        {
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .With(x => x.Email, string.Empty)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsMissing()
        {
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .Without(x => x.Email)
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task ThrowValidationExceptionWhenEmailIsNotValid()
        {
            var fixture = new Fixture();
            var createUser = fixture.Build<CreateUser.Command>()
                .Create();

            await Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(createUser));
        }

        [Fact]
        public async Task CallCreateUserWhenProvidingValidData()
        {
            var fixture = new Fixture();

            var createUser = fixture.Build<CreateUser.Command>()
                .With(x => x.Email, fixture.Create<MailAddress>().Address)
                .Create();

            var result = await _mediator.Send(createUser);

            result.Should().NotBeNull();

            _jobClientMock.Verify(x => 
                x.Create(It.Is<Job>(job => 
                    job.Method.Name == nameof(CreateUser.CreateUserHandler.CreateUser)), It.IsAny<EnqueuedState>()));
        }

        [Fact]
        public void CreateUserOutsideHandler()
        {
            var fixture = new Fixture();

            var expectedUser = fixture.Build<DataModel.Models.User>()
                .With(x => x.Email, fixture.Create<MailAddress>().Address)
                .Create();

            var mapper = new Mock<IMapper>();
            var createUser = new CreateUser.CreateUserHandler(_db, mapper.Object, _jobClientMock.Object, _mediator);

            createUser.CreateUser(expectedUser);

            var actualUser = _db.Users.FirstOrDefault();

            actualUser.Should().BeEquivalentTo(expectedUser);
        }
    }
}
