using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public interface IMediaService
    {
        Task<IEnumerable<Media>> GetAllMedias();
        Task<Media?> GetMediaById(int id);
        Task<int> CreateMedia(Media Media);
        Task<int> UpdateMedia(Media Media);
        Task<int> DeleteMedia(int id);
    }
}