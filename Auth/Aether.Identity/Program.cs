using Aether.Identity.Context;
using Aether.Identity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Text;

namespace Aether.Identity
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
            builder.Services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("SampleAppDatabase"));
            GenerateDummyData(builder.Services);

            var app = builder.Build();

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

        private static void GenerateDummyData(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var dbContext = provider.GetService<IdentityDbContext>();
            var userDb = dbContext.Set<User>();

            int generatedUser = 5;
            for (int i = 0; i < generatedUser; i++)
            {
                var user = new User()
                {
                    Username = $"test{i}"
                };
                using (var hmac = new HMACSHA512())
                {
                    user.PasswordSalt = hmac.Key;
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                }
                userDb.Add(user);
            }
            dbContext.SaveChanges();
        }
    }
}