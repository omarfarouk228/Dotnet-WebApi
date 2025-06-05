using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Data.Interfaces
{
    public interface IMediaRepository
    {
        Task<IEnumerable<Media>> GetAllMediasAsync();
        Task<Media?> GetMediaByIdAsync(int id);
        Task<int> CreateMediaAsync(Media media);
        Task<int> UpdateMediaAsync(Media media);
        Task<int> DeleteMediaAsync(int id);
    }
}