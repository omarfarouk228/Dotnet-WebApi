using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResult> RegisterAsync(string username, string email, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<AuthResult> ForgotPasswordAsync(string email);
        Task<AuthResult> ResetPasswordAsync(string token, string password);
    }
}