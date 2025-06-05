using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string username, string email, string password);
        Task<AuthResult> LoginAsync(string email, string password);
    }
}