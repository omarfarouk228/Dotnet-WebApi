using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}