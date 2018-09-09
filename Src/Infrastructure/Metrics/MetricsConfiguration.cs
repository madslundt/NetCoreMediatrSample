using App.Metrics;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Formatters.Json;
using App.Metrics.Reporting.Elasticsearch;
using App.Metrics.Reporting.Elasticsearch.Client;
using App.Metrics.Reporting.Graphite;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Src.Infrastructure.Metrics
{
    public static class MetricsConfiguration
    {
        private readonly static IFilterMetrics _filter = new MetricsFilter().WhereType(MetricType.Timer);

        public static IServiceCollection AddMetricsConfiguration(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var reportingConfiguration = configuration.GetSection("MetricsReporting");

            var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(options =>
                {
                    options.WithGlobalTags((tags, envInfo) =>
                    {
                        tags.Add("app_version", envInfo.EntryAssemblyVersion);
                    });
                });

            var influxDb = new InfluxDb(reportingConfiguration.GetSection("InfluxDb"));
            if (influxDb.IsEnabled)
            {
                metrics.Report.ToInfluxDb(options => influxDb.GetOptions(options));
            }

            var elasticsearch = new Elasticsearch(reportingConfiguration.GetSection("Elasticsearch"));
            if (elasticsearch.IsEnabled)
            {
                metrics.Report.ToElasticsearch(options => elasticsearch.GetOptions(options));
            }

            var graphite = new Graphite(reportingConfiguration.GetSection("Graphite"));
            if (graphite.IsEnabled)
            {
                metrics.Report.ToGraphite(options => graphite.GetOptions(options));
            }
            
            metrics.Build();
            
            services
                .AddMetricsTrackingMiddleware()
                .AddMetricsEndpoints()
                .AddMetrics(metrics);

            return services;
        }

        private class InfluxDb
        {
            public string Url { get; }
            public string Database { get; }
            public string UserName { get; }
            public string Password { get; }

            public bool IsEnabled { get; }

            public InfluxDb(IConfigurationSection configuration)
            {
                Url = configuration.GetSection(nameof(Url)).Value;
                Database = configuration.GetSection(nameof(Database)).Value;
                UserName = configuration.GetSection(nameof(UserName)).Value;
                Password = configuration.GetSection(nameof(Password)).Value;
            }

            public MetricsReportingInfluxDbOptions GetOptions(MetricsReportingInfluxDbOptions options)
            {
                options.InfluxDb.BaseUri = new Uri(Url);
                options.InfluxDb.Database = Database;
                if (!string.IsNullOrWhiteSpace(UserName))
                {
                    options.InfluxDb.UserName = UserName;
                }
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    options.InfluxDb.Password = Password;
                }
                options.InfluxDb.Consistenency = "consistency";
                options.InfluxDb.RetentionPolicy = "rp";
                options.InfluxDb.CreateDataBaseIfNotExists = true;
                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.HttpPolicy.FailuresBeforeBackoff = 5;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                options.Filter = _filter;
                options.FlushInterval = TimeSpan.FromSeconds(20);

                return options;
            }
        }

        private class Elasticsearch
        {
            public string Url { get; set; }
            public string Index { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string BearerToken { get; set; }

            public bool IsEnabled { get { return !string.IsNullOrWhiteSpace(Url); } }

            public Elasticsearch(IConfigurationSection configuration)
            {
                Url = configuration.GetSection(nameof(Url)).Value;
                Index = configuration.GetSection(nameof(Index)).Value;
                UserName = configuration.GetSection(nameof(UserName)).Value;
                Password = configuration.GetSection(nameof(Password)).Value;
                BearerToken = configuration.GetSection(nameof(BearerToken)).Value;
            }

            public MetricsReportingElasticsearchOptions GetOptions(MetricsReportingElasticsearchOptions options)
            {
                options.Elasticsearch.Index = Index;
                options.Elasticsearch.BaseUri = new Uri(Url);
                if (!string.IsNullOrWhiteSpace(UserName) || !string.IsNullOrWhiteSpace(Password))
                {
                    options.Elasticsearch.UserName = UserName;
                    options.Elasticsearch.Password = Password;
                    options.Elasticsearch.AuthorizationSchema = ElasticSearchAuthorizationSchemes.Basic;
                }
                else if (!string.IsNullOrWhiteSpace(BearerToken))
                {
                    options.Elasticsearch.BearerToken = BearerToken;
                    options.Elasticsearch.AuthorizationSchema = ElasticSearchAuthorizationSchemes.BearerToken;
                }
                else
                {
                    options.Elasticsearch.AuthorizationSchema = ElasticSearchAuthorizationSchemes.Anonymous;
                }
                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.HttpPolicy.FailuresBeforeBackoff = 5;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                options.Filter = _filter;
                options.FlushInterval = TimeSpan.FromSeconds(20);

                return options;
            }
        }

        private class Graphite
        {
            public string Url { get; set; }

            public bool IsEnabled { get { return !string.IsNullOrWhiteSpace(Url); } }

            public Graphite(IConfigurationSection configuration)
            {
                Url = configuration.GetSection(nameof(Url)).Value;
            }

            public MetricsReportingGraphiteOptions GetOptions(MetricsReportingGraphiteOptions options)
            {
                options.Graphite.BaseUri = new Uri(Url);
                options.ClientPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.ClientPolicy.FailuresBeforeBackoff = 5;
                options.ClientPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.Filter = _filter;
                options.FlushInterval = TimeSpan.FromSeconds(20);

                return options;
            }
        }
    }
}
