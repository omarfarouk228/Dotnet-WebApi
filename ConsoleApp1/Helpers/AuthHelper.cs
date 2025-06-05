using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp1.Helpers
{
    public class AuthHelper
    {
        public static string HashPassword(string password, string salt)
        {
            var saltedPassword = password + salt;
            var hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(saltedPassword));

            return Convert.ToBase64String(hashBytes);
        }

        public static string GenerateJwtToken(User user)
        {
            var jwtKey = "MediaApiTokenGeneration123456789@";
            var jwtIssuer = "AuthAPI";
            var jwtAudience = "BlazorApp";

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Subject = new System.Security.Claims.ClaimsIdentity([
                    new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new System.Security.Claims.Claim(ClaimTypes.Name, user.Username.ToString()),
                ]),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);



        }

        public static string GenerateSalt()
        {
            using var ng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var saltBytes = new byte[32];
            ng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }
    }
}