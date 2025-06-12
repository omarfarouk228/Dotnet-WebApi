using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int FolderId { get; set; }
        public string? Content { get; set; }
    }

    public class FileCreate
    {
        public string Name { get; set; } = "";
        public int FolderId { get; set; }
        public string? Content { get; set; }
    }

    public class FileUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int FolderId { get; set; }
        public string? Content { get; set; }
    }
}