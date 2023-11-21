using Aether.SampleApplication.Context;
using Aether.SampleApplication.Entities;
using Aether.SampleApplication.Identity;
using Aether.SampleApplication.Swagger;
using Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Aether.SampleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityData.AdminUserPolicyName, p => 
                    p.RequireClaim(IdentityData.AdminUserClaimName, "true")    
                );
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddDbContext<SampleAppDbContext>(options => options.UseInMemoryDatabase("SampleAppDatabase"));
            GenerateDummyData(builder.Services);

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void GenerateDummyData(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var dbContext = provider.GetService<SampleAppDbContext>();
            var songDb = dbContext.Set<Song>();

            var years = new int[] { 2019, 2020, 2021, 2022, 2023 };
            var songGenerator = new Faker<Song>()
                .RuleFor(song => song.Name, faker => faker.Commerce.ProductName())
                .RuleFor(song => song.Artist, faker => faker.Name.FullName())
                .RuleFor(song => song.Genre, faker => faker.Music.Genre())
                .RuleFor(song => song.Year, faker => faker.PickRandom(years));

            var songs = songGenerator.Generate(20);
            songDb.AddRange(songs);
            dbContext.SaveChanges();
        }
    }
}