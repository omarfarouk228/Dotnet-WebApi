using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.dto
{
    public class MediaUploadDto
    {
        public string Name { get; set; } = string.Empty;
        public int GroupID { get; set; }
        public string? Type { get; set; }
        public int Status { get; set; }

        public IFormFile File { get; set; } = null!;
    }
}