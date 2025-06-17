using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public interface IFolderService
    {
        Task<IEnumerable<Folder>> GetAllFolders();
        Task<Folder?> GetFolderById(int id);
        Task<IEnumerable<Folder>> GetChildrens(int id);
        Task<int> CreateFolder(FolderCreate Folder);
        Task<int> UpdateFolder(FolderUpdate Folder);
        Task<int> DeleteFolder(int id);
        Task<bool> HasFiles(int id);
        Task<bool> HasChildrens(int id);

    }
}