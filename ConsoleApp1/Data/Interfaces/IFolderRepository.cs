using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Data.Interfaces
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllFolders();
        Task<Folder?> GetFolderById(int id);
        Task<IEnumerable<Folder>> GetChildrens(int id);
        Task<int> CreateFolder(FolderCreate Folder);
        Task<int> UpdateFolder(FolderUpdate Folder);
        Task<int> DeleteFolder(int id);
        Task<bool> HasFiles(int id);
    }
}