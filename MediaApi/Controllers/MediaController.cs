using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using MediaApi.dto;
using MediaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MediaController(IMediaService mediaService, IWebHostEnvironment env) : ControllerBase
    {
        private readonly IMediaService _mediaService = mediaService;
        private readonly string _uploadPath = Path.Combine(env.ContentRootPath ?? "wwwroot", "uploads");

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Media>>> GetAll()
        {
            var medias = await _mediaService.GetAllMedias();
            return Ok(medias);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<int>> Create(
            [FromForm] MediaUploadDto mediaUploadDto)
        {
            string? fileName = null;

            if (mediaUploadDto.File != null && mediaUploadDto.File.Length > 0)
            {
                // Créer le dossier de stockage s'il n'existe pas
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                fileName = Guid.NewGuid() + Path.GetExtension(mediaUploadDto.File.FileName);
                var filePath = Path.Combine(_uploadPath, fileName);

                // Sauvegarder le fichier
                using var stream = new FileStream(filePath, FileMode.Create);
                await mediaUploadDto.File.CopyToAsync(stream);
            }


            var media = new Media
            {
                Name = mediaUploadDto.Name,
                Type = mediaUploadDto.Type,
                GroupID = mediaUploadDto.GroupID,
                Status = mediaUploadDto.Status,
                Path = fileName is not null ? $"/uploads/{fileName}" : null
            };


            var id = await _mediaService.CreateMedia(media);
            return Ok(id);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Media?>> Get(int id)
        {
            var media = await _mediaService.GetMediaById(id);
            return Ok(media);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<int>> Update(int id, [FromForm] MediaUpdateDto mediaUpdateDto)
        {
            if (id != mediaUpdateDto.ID)
            {
                return BadRequest("Id non correspondant");
            }

            string? fileName = null;

            if (mediaUpdateDto.File != null && mediaUpdateDto.File.Length > 0)
            {
                // Créer le dossier de stockage s'il n'existe pas
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                fileName = Guid.NewGuid() + Path.GetExtension(mediaUpdateDto.File.FileName);
                var filePath = Path.Combine(_uploadPath, fileName);

                // Sauvegarder le fichier
                using var stream = new FileStream(filePath, FileMode.Create);
                await mediaUpdateDto.File.CopyToAsync(stream);
            }


            var media = new Media
            {
                ID = mediaUpdateDto.ID,
                Name = mediaUpdateDto.Name,
                Type = mediaUpdateDto.Type,
                GroupID = mediaUpdateDto.GroupID,
                Status = mediaUpdateDto.Status,
                Path = fileName is not null ? $"/uploads/{fileName}" : null
            };

            var result = await _mediaService.UpdateMedia(media);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _mediaService.DeleteMedia(id);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}