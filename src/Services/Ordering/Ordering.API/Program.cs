using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.API.Extensions;
using Ordering.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Serilog;
using Ordering.Application.Constants;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Ordering.Application.Models;
using Microsoft.Extensions.Configuration;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<ApplicationDbContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
                    ApplicationDbContextSeed.SeedAsync(context, logger).Wait();
                })
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, config) =>
                {
                    var serilogEsConfig = context.Configuration
                       .GetSection(AppConstants.SERILOG_ELASTIC_SEARCH_CONFIGURATION)
                       .Get<SerilogElasticSearchConfig>();

                    Enum.TryParse(context.Configuration[AppConstants.SERILOG_MINIMUM_LOGGING_LEVEL], out LogEventLevel logLevelEnum);

                    config.Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(
                            new ElasticsearchSinkOptions(new Uri(serilogEsConfig.Uri))
                            {
                                IndexFormat = ReturnIndexFormat(context),
                                AutoRegisterTemplate = true,
                                MinimumLogEventLevel = logLevelEnum
                            })
                        .Enrich.WithProperty(AppConstants.ENVIRONMENT, context.HostingEnvironment.EnvironmentName)
                        .ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static string ReturnIndexFormat(HostBuilderContext context) =>
            $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
    }
}
