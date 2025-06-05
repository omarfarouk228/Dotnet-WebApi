using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Media
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string? Type { get; set; }
        public int Status { get; set; }
        public int GroupID { get; set; }
        public string? Path { get; set; }
    }
}