using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.dto
{
    public class LoginDto
    {
        [DefaultValue("komarf28@gmail.com")]
        public string Email { get; set; } = string.Empty;

        [DefaultValue("123456789")]
        public string Password { get; set; } = string.Empty;
    }
}