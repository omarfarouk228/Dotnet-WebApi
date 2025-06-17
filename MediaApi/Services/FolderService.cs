using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public class FolderService(IFolderRepository folderRepository) : IFolderService
    {
        private readonly IFolderRepository _folderRepository = folderRepository;

        public Task<int> CreateFolder(FolderCreate Folder) => _folderRepository.CreateFolder(Folder);

        public Task<int> DeleteFolder(int id) => _folderRepository.DeleteFolder(id);

        public Task<IEnumerable<Folder>> GetAllFolders() => _folderRepository.GetAllFolders();

        public Task<IEnumerable<Folder>> GetChildrens(int id) => _folderRepository.GetChildrens(id);

        public Task<Folder?> GetFolderById(int id) => _folderRepository.GetFolderById(id);

        public Task<bool> HasFiles(int id) => _folderRepository.HasFiles(id);

        public Task<bool> HasChildrens(int id) => _folderRepository.HasChildrens(id);

        public Task<int> UpdateFolder(FolderUpdate Folder) => _folderRepository.UpdateFolder(Folder);
    }
}