using System;
using System.Threading.Tasks;
using AutoFixture;
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
    public class GetUserTest
    {
        private readonly IMediator _mediator;

        public GetUserTest()
        {
            var services = new ServiceCollection();
            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMvc().AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            _mediator = services.BuildServiceProvider().GetService<IMediator>();
        }

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
        public void GetUserWhenProvidingAValidId()
        {
            var fixture = new Fixture();
            var getUser = fixture.Build<GetUser.Query>()
                .Create();

            var result = _mediator.Send(getUser);

            Assert.NotNull(result);
        }


    }
}
