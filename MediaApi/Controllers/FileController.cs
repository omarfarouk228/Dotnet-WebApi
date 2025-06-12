using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using MediaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FileController(IFileService fileService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;

        [HttpGet("folder/{folderId:int}")]
        public async Task<ActionResult<IEnumerable<ConsoleApp1.Models.File>>> GetFilesByFolderId(int folderId)
        {
            var files = await _fileService.GetFilesByFolderId(folderId);
            return Ok(files);
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] FileCreate file)
        {
            var id = await _fileService.CreateFile(file);
            return Ok(id);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ConsoleApp1.Models.File?>> Get(int id)
        {
            var file = await _fileService.GetFileById(id);
            return Ok(file);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] FileUpdate file)
        {
            if (id != file.Id)
            {
                return BadRequest("Id non correspondant");
            }
            var result = await _fileService.UpdateFile(file);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _fileService.DeleteFile(id);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}