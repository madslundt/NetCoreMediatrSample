using AutoMapper;
using DataModel;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Src;
using Src.Features.User;
using Src.Infrastructure.Pipeline;
using System;
using System.Reflection;

namespace Test.Common
{
    public class TestBaseInMemoryDatabase : IDisposable
    {
        protected readonly IMediator _mediator;
        protected readonly DatabaseContext _db;

        public TestBaseInMemoryDatabase()
        {

            var services = new ServiceCollection();
            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMvc().AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            var databaseName = Guid.NewGuid().ToString();
            _db = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options);

            services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase(databaseName), ServiceLifetime.Transient);
            services.AddTransient(sp => sp.GetService<DatabaseContext>());

            _mediator = services.BuildServiceProvider().GetService<IMediator>();
        }

        public void Dispose()
        {}
    }
}
