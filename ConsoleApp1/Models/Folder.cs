using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int? ParentId { get; set; }
        public List<Folder> SubFolders { get; set; } = [];
        public List<File> Files { get; set; } = [];
    }

    public class FolderCreate
    {
        public string Name { get; set; } = "";

        public int? ParentId { get; set; }
    }

    public class FolderUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int? ParentId { get; set; }
    }
}