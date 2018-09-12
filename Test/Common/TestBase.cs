using AutoMapper;
using DataModel;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Src;
using Src.Infrastructure.Pipeline;
using StructureMap;
using System;

namespace Test.Common
{
    public class TestBase : IDisposable
    {
        protected readonly IMediator _mediator;
        protected readonly DatabaseContext _db;
        protected readonly Mock<IBackgroundJobClient> _jobClientMock;

        public TestBase()
        {

            var services = new ServiceCollection();
            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMvc().AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            var databaseName = Guid.NewGuid().ToString();

            _db = new DatabaseContext(DatabaseContextMock<DatabaseContext>.InMemoryDatabase());

            _jobClientMock = new Mock<IBackgroundJobClient>();

            _jobClientMock.Setup(x => x.Create(It.IsAny<Job>(), It.IsAny<EnqueuedState>()));

            services.AddAutoMapper();

            IContainer container = new Container(cfg =>
            {
                cfg.For<IBackgroundJobClient>().Use(_jobClientMock.Object);
                cfg.For<DatabaseContext>().Use(_db);
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
