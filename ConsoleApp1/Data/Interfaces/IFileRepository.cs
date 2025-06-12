using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Data.Interfaces
{
    public interface IFileRepository
    {
        Task<IEnumerable<Models.File>> GetFilesByFolderId(int id);
        Task<Models.File?> GetFileById(int id);
        Task<int> CreateFile(FileCreate File);
        Task<int> UpdateFile(FileUpdate File);
        Task<int> DeleteFile(int id);
    }
}