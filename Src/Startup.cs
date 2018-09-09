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
using App.Metrics;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Filtering;

namespace Src
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
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

            // Pipeline
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MetricsProcessor<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ExceptionFilter));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            var filter = new MetricsFilter().WhereType(MetricType.Timer);
            var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(options =>
                {
                    options.WithGlobalTags((tags, envInfo) =>
                    {
                        tags.Add("app_version", envInfo.EntryAssemblyVersion);
                    });
                })
                .Report.ToInfluxDb(options =>
                {
                    options.InfluxDb.BaseUri = new Uri(Configuration.GetSection("MetricsReporting:InfluxDb:Url").Value);
                    options.InfluxDb.Database = Configuration.GetSection("MetricsReporting:InfluxDb:Database").Value;
                    options.InfluxDb.UserName = Configuration.GetSection("MetricsReporting:InfluxDb:UserName").Value;
                    options.InfluxDb.Password = Configuration.GetSection("MetricsReporting:InfluxDb:Password").Value;
                    options.InfluxDb.Consistenency = "consistency";
                    options.InfluxDb.RetensionPolicy = "rp";
                    options.InfluxDb.CreateDataBaseIfNotExists = true;
                    options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    options.HttpPolicy.FailuresBeforeBackoff = 5;
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                    options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                    options.Filter = filter;
                    options.FlushInterval = TimeSpan.FromSeconds(20);
                })
                .Build();

            metrics.ReportRunner.RunAllAsync();
            services
                .AddMetrics(metrics)
                .AddHealthEndpoints();

            IContainer container = new Container();
            container.Configure(config =>
            {
                config.AddRegistry<HelperRegistry>();

                config.Populate(services);
            });

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

            app.UseMetricsAllMiddleware();
            app.UseMetricsAllEndpoints();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            app.UseMvc();
        }
    }
}
