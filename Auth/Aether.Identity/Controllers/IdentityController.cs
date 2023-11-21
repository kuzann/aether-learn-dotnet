using Aether.Identity.Context;
using Aether.Identity.Dto;
using Aether.Identity.Entities;
using Aether.Identity.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Aether.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityDbContext _dbContext;
        private readonly DbSet<User> _dbSet;

        private const string TokenSecret = "ThisIsDefinitelyNotALongTokenSecret";
        private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(1);

        public IdentityController(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<User>();
        }

        [HttpPost("token")]
        public IActionResult GenerateToken(TokenGenerationRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                new Claim(JwtRegisteredClaimNames.Email, request.Email),
                new Claim("userid", request.UserId.ToString()),
            };

            foreach (var claimPair in request.CustomClaims)
            {
                var valueType = claimPair.Value.ValueKind switch
                {
                    JsonValueKind.Number => ClaimValueTypes.Double,
                    JsonValueKind.True => ClaimValueTypes.Boolean,
                    JsonValueKind.False => ClaimValueTypes.Boolean,
                    _ => ClaimValueTypes.String,
                };

                claims.Add(new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType));
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = "https://localhost:5000",
                Audience = "https://localhost:5000",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(tokenHandler.WriteToken(token));
        }

        [HttpPost("register")]
        public IActionResult RegisterUser(UserDto request)
        {
            if (request == null)
                return BadRequest("Request cannot be null");

            if (!string.IsNullOrWhiteSpace(request.Username))
                return BadRequest("Username cannot be empty");

            if (!string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Password cannot be empty");

            var user = new User
            {
                Username = request.Username
            };
            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            }
            _dbSet.Add(user);
            _dbContext.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto request)
        {
            var user = _dbSet.FirstOrDefault(u => u.Username == request.Username);
            if (user == null)
                return BadRequest("User not found");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            string token = CreateToken(user);

            return Ok(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSecret));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddDays(1),
                                   signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
