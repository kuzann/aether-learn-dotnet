using Aether.Kubernetes.Api.DependencyInjections;
using Aether.Kubernetes.Api.Infrastructure;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Aether.Kubernetes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Builder

            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;
            IWebHostEnvironment environment = builder.Environment;


            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationDatabase(configuration);

            #endregion

            #region Application

            WebApplication app = builder.Build();

            // Apply migrations here using the service scope to ensure correct DI resolution
            app.MigrateApplicationDatabase();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/", () => "Hello from Web API!");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            #endregion
        }
    }
}
