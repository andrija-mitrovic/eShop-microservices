using Discount.API.Extensions;
using Discount.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace Discount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.MigrateDatabase<Program>();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, config) =>
                {
                    var serilogEsConfig = context.Configuration
                       .GetSection(Constants.SERILOG_ELASTIC_SEARCH_CONFIGURATION)
                       .Get<SerilogElasticSearchConfig>();

                    Enum.TryParse(context.Configuration[Constants.SERILOG_MINIMUM_LOGGING_LEVEL], out LogEventLevel logLevelEnum);

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
                        .Enrich.WithProperty(Constants.ENVIRONMENT, context.HostingEnvironment.EnvironmentName)
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
