using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Helpers;
using ConsoleApp1.Models;
using Dapper;

namespace ConsoleApp1.Data.Repositories
{
    public class AuthRepository(IDbConnectionFactory db) : IAuthRepository
    {
        private readonly IDbConnectionFactory _db = db;

        public async Task<AuthResult> ForgotPasswordAsync(string email)
        {
            using var connection = await _db.GetOpenConnectionInterface();

            var sql = "SELECT * FROM Users WHERE Email = @Email AND isActive = 1";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });

            if (user == null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "Utilisateur non trouvé ou désactivé."
                };
            }

            // Génération de Token
            var token = Uri.EscapeDataString(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
            var tokenExpiration = DateTime.Now.AddHours(1);

            // Mise à jour dans la BD
            var updateSql = "UPDATE Users SET ResetToken = @Token, ResetTokenExpires = @TokenExpiration WHERE Id = @Id";
            await connection.ExecuteAsync(updateSql, new { user.Id, Token = token, TokenExpiration = tokenExpiration });

            return new AuthResult { IsSuccess = true, Message = "Lien de réinitialisation envoyé par mail.", Token = token };
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            using var connection = await _db.GetOpenConnectionInterface();

            var sql = "SELECT * FROM Users WHERE Email = @Email AND isActive = 1";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });

            if (user == null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "Utilisateur non trouvé ou désactivé."
                };
            }

            var passwordHash = AuthHelper.HashPassword(password, user.Salt);
            if (user.PasswordHash != passwordHash)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "Mot de passe incorrect."
                };
            }

            // Mettre à jour LastLogin
            await connection.ExecuteAsync("UPDATE Users SET LastLogin = @LastLogin WHERE Id = @Id",
            new { user.Id, LastLogin = DateTime.Now });

            var token = AuthHelper.GenerateJwtToken(user);

            return new AuthResult
            {
                IsSuccess = true,
                Message = "Connexion réussie.",
                User = user,
                Token = token
            };
        }

        public async Task<AuthResult> RegisterAsync(string username, string email, string password)
        {
            using var connection = await _db.GetOpenConnectionInterface();

            // Vérifier si l'utilisateur existe déjà
            var existingUser = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email",
             new { Email = email });

            if (existingUser != null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "L'utilisateur existe déja."
                };
            }

            var salt = AuthHelper.GenerateSalt();
            var passwordHash = AuthHelper.HashPassword(password, salt);

            // Insertion dans la base
            var insertQuery = @"INSERT INTO Users(Username, Email, PasswordHash, Salt, CreatedAt, IsActive) OUTPUT INSERTED.* 
            VALUES(@Username, @Email, @PasswordHash, @Salt, @CreatedAt, @IsActive)";

            var user = await connection.QueryFirstAsync<User>(insertQuery, new
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Salt = salt,
                CreatedAt = DateTime.Now,
                IsActive = true
            });

            var token = AuthHelper.GenerateJwtToken(user);

            return new AuthResult
            {
                IsSuccess = true,
                Message = "Inscription réussie.",
                User = user,
                Token = token
            };
        }

        public async Task<AuthResult> ResetPasswordAsync(string token, string password)
        {
            Console.WriteLine(token);
            Console.WriteLine(password);
            using var connection = await _db.GetOpenConnectionInterface();

            var sql = "SELECT * FROM Users WHERE ResetToken = @ResetToken AND IsActive = 1";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { ResetToken = Uri.EscapeDataString(token) });

            if (user == null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "Utilisateur non trouvé ou désactivé."
                };
            }

            if (user.ResetTokenExpires < DateTime.UtcNow)
                return new AuthResult { IsSuccess = false, Message = "Token invalide ou expiré." };

            var salt = AuthHelper.GenerateSalt();
            var passwordHash = AuthHelper.HashPassword(password, salt);

            var sqlUpdatePassword = @"UPDATE Users SET PasswordHash = @PasswordHash, Salt = @Salt, ResetToken = NULL, ResetTokenExpires = NULL WHERE Id = @Id";
            await connection.ExecuteAsync(sqlUpdatePassword, new { user.Id, PasswordHash = passwordHash, Salt = salt });

            return new AuthResult
            {
                IsSuccess = true,
                Message = "Mot de passe mis à jour.",
            };
        }
    }
}