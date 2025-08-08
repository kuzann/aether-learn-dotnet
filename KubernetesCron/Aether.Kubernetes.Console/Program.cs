using Aether.Kubernetes.Api;
using Aether.Kubernetes.Api.DataProviders;
using Aether.Kubernetes.Api.DependencyInjections;
using Aether.Kubernetes.Api.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aether.Kubernetes.Console
{
    internal class Program
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        static async Task Main(string[] args)
        {
            System.Console.WriteLine($"Cron Job started at {DateTime.UtcNow}");
            System.Console.WriteLine($"Arguments: {JsonSerializer.Serialize(args)}");

            //var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //}).ToArray();
            //System.Console.WriteLine(JsonSerializer.Serialize(data));

            // Set up dependency injection

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            string connection = configuration.GetConnectionString("ApplicationConnection").Replace("localhost", "host.docker.internal").Replace("127.0.0.1", "host.docker.internal");
            var services = new ServiceCollection();
            services.AddApplicationDatabase(configuration, connection);
            services.AddScoped<IVendorDataProvider, VendorDataProvider>();
            services.AddLogging();

            var logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger(typeof(Program));
            logger.LogInformation(configuration.GetConnectionString("ApplicationConnection"));
            logger.LogInformation($"REPLACED: {connection}");

            var serviceProvider = services.BuildServiceProvider();

            // Get the required service
            IVendorDataProvider vendorData = serviceProvider.GetService<IVendorDataProvider>();

            // Use the service in background job
            var vendors = await vendorData.List();
            System.Console.WriteLine(JsonSerializer.Serialize(vendors));

            System.Console.WriteLine($"Cron Job finished");
        }
    }
}
