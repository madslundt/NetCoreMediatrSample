using System;
using API;
using API.Infrastructure.Pipeline;
using AutoFixture;
using AutoMapper;
using DataModel;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StructureMap;

namespace UnitTest.Common
{
    public class TestBase : IDisposable
    {
        protected readonly IMediator _mediator;
        protected readonly DatabaseContext _db;
        protected readonly Mock<IBackgroundJobClient> _jobClientMock;
        protected readonly Fixture _fixture;

        public TestBase()
        {
            var services = new ServiceCollection();

            // Services
            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMvc().AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            services.AddAutoMapper();


            // Database
            var databaseName = Guid.NewGuid().ToString();
            _db = new DatabaseContext(DatabaseContextMock<DatabaseContext>.InMemoryDatabase());


            // Global objects
            _jobClientMock = new Mock<IBackgroundJobClient>();
            _jobClientMock.Setup(x => x.Create(It.IsAny<Job>(), It.IsAny<EnqueuedState>()));

            _fixture = new Fixture();


            IContainer container = new Container(cfg =>
            {
                cfg.For<IBackgroundJobClient>().Use(_jobClientMock.Object);
                cfg.For<DatabaseContext>().Use(_db);
                cfg.For(typeof(ILogger<>)).Use(typeof(NullLogger<>));
                cfg.Populate(services);
            });

            _mediator = container.GetInstance<IMediator>();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
