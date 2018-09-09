using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Src.Infrastructure.Pipeline;
using FluentValidation.AspNetCore;
using MediatR.Pipeline;
using Src.Infrastructure.Filter;
using Hangfire;
using DataModel;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using Src.Infrastructure.Registry;
using System;
using CorrelationId;
using Microsoft.Extensions.Logging;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics;
using App.Metrics.AspNetCore;

namespace Src
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(typeof(Startup));
            
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString(ConnectionStringKeys.App)));
            
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString(ConnectionStringKeys.Hangfire)));

            services.AddCorrelationId();

            services.AddOptions();

            var metricsConfigSection = Configuration.GetSection(nameof(MetricsOptions));
            var influxOptions = new MetricsReportingInfluxDbOptions();
            Configuration.GetSection(nameof(MetricsReportingInfluxDbOptions)).Bind(influxOptions);

            var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(metricsConfigSection.AsEnumerable())
                .Report.ToInfluxDb(influxOptions)
                .Build();

            services.AddMetrics(metrics);
            services.AddMetricsReportScheduler();

            // Pipeline
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MetricsProcessor<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ExceptionFilter));
            })
            .AddMetrics()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            IContainer container = new Container();
            container.Configure(config =>
            {
                config.AddRegistry<HelperRegistry>();

                config.Populate(services);
            });

            metrics.ReportRunner.RunAllAsync();

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseCorrelationId(new CorrelationIdOptions
            {
                UseGuidForCorrelationId = true
            });

            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }

            app.UseMetricsAllEndpoints();
            app.UseMetricsAllMiddleware();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseMvc();
        }
    }
}
