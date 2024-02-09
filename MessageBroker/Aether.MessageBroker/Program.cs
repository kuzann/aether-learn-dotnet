using Aether.MessageBroker.Services.RabbitMQ;
using Aether.MessageBroker.Services.RabbitMQ.Scoped;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Aether.MessageBroker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<RabbitMQConnectionPool>();
            builder.Services.AddScoped<MessageConsumer>();
            builder.Services.AddScoped<MessagePublisher>();

            // testing scoped
            builder.Services.AddScoped<ConsumerScoped>();
            builder.Services.AddScoped<PublisherScoped>();

            var app = builder.Build();

            InitializeRabbitMqConsumer(app.Services);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void InitializeRabbitMqConsumer(IServiceProvider serviceProvider)
        {
            var connectionPool = serviceProvider.GetService<RabbitMQConnectionPool>();
            connectionPool.InitializeConsumer();
        }
    }
}