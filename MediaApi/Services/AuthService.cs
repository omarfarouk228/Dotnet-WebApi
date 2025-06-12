using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;

        public Task<AuthResult> ForgotPasswordAsync(string email) => _authRepository.ForgotPasswordAsync(email);

        public Task<AuthResult> LoginAsync(string email, string password) => _authRepository.LoginAsync(email, password);

        public Task<AuthResult> RegisterAsync(string username, string email, string password) => _authRepository.RegisterAsync(username, email, password);

        public Task<AuthResult> ResetPasswordAsync(string token, string password) => _authRepository.ResetPasswordAsync(token, password);
    }
}