using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Data.Interfaces;
using ConsoleApp1.Models;

namespace MediaApi.Services
{
    public class MediaService(IMediaRepository mediaRepository) : IMediaService
    {
        private readonly IMediaRepository _mediaRepository = mediaRepository;

        public Task<IEnumerable<Media>> GetAllMedias() => _mediaRepository.GetAllMediasAsync();

        public Task<Media?> GetMediaById(int id) => _mediaRepository.GetMediaByIdAsync(id);


        public Task<int> CreateMedia(Media media) => _mediaRepository.CreateMediaAsync(media);

        public Task<int> UpdateMedia(Media media) => _mediaRepository.UpdateMediaAsync(media);

        public Task<int> DeleteMedia(int id) => _mediaRepository.DeleteMediaAsync(id);


    }
}