using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public User? User { get; set; }
        public string? Token { get; set; }

    }
}