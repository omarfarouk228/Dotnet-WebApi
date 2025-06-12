using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public class FileService(IFileRepository fileRepository) : IFileService
    {
        private readonly IFileRepository _fileRepository = fileRepository;
        public Task<int> CreateFile(FileCreate File) => _fileRepository.CreateFile(File);

        public Task<int> DeleteFile(int id) => _fileRepository.DeleteFile(id);

        public Task<ConsoleApp1.Models.File?> GetFileById(int id) => _fileRepository.GetFileById(id);

        public Task<IEnumerable<ConsoleApp1.Models.File>> GetFilesByFolderId(int id) => _fileRepository.GetFilesByFolderId(id);

        public Task<int> UpdateFile(FileUpdate File) => _fileRepository.UpdateFile(File);
    }
}