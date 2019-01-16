using System;
using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile(path: $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                                .Build();

            var influxOptions = new MetricsReportingInfluxDbOptions();
            configuration.GetSection(nameof(MetricsReportingInfluxDbOptions)).Bind(influxOptions);

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel((context, options) =>
                {
                    // Set properties and call methods on options
                })
                .ConfigureMetricsWithDefaults(
                    builder =>
                    {
                        builder.Report.ToInfluxDb(influxOptions);
                    })
                .UseMetrics()
                .UseStartup<Startup>()
                .Build();
        }
    }
}
