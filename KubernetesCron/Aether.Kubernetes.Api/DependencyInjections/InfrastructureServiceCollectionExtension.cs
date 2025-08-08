using Aether.Kubernetes.Api.Infrastructure;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Aether.Kubernetes.Api.DependencyInjections
{
    public static class InfrastructureServiceCollectionExtension
    {
        /// <summary>
        /// Add Application Database and its migration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration, string connection = null)
        {
            connection = string.IsNullOrWhiteSpace(connection) ? configuration.GetConnectionString("ApplicationConnection") : connection;

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(connection);
                options.UseExceptionProcessor();
                options.EnableDetailedErrors();
            });
        }


        /// <summary>
        /// Add Migrations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void MigrateApplicationDatabase(this WebApplication app)
        {
            // Apply migrations here using the service scope to ensure correct DI resolution
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
