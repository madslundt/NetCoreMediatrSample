using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Src;
using Src.Features.User;
using Src.Infrastructure.Pipeline;
using Xunit;

namespace Test.Features.User
{
    public class CreateUserTest
    {
        private readonly IMediator _mediator;

        public CreateUserTest()
        {
            var services = new ServiceCollection();
            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMvc().AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            _mediator = services.BuildServiceProvider().GetService<IMediator>();
        }

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
        public void CreateUserWhenProvidingValidData()
        {
            var fixture = new Fixture();

            var createUser = fixture.Build<CreateUser.Command>()
                .Without(x => x.FirstName)
                .With(x => x.Email, fixture.Create<MailAddress>().Address)
                .Create();

            var result = _mediator.Send(createUser);

            Assert.NotNull(result);
        }


    }
}
