using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.dto
{
    public class ResetDto
    {
        [DefaultValue("kvbsbaàç90190909190191")]
        public string Token { get; set; } = string.Empty;

        [DefaultValue("123456789")]
        public string Password { get; set; } = string.Empty;
    }
}