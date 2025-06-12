using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public interface IFileService
    {
        Task<IEnumerable<ConsoleApp1.Models.File>> GetFilesByFolderId(int id);
        Task<ConsoleApp1.Models.File?> GetFileById(int id);
        Task<int> CreateFile(FileCreate File);
        Task<int> UpdateFile(FileUpdate File);
        Task<int> DeleteFile(int id);
    }
}