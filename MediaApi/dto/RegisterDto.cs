using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.dto
{
    public class RegisterDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}