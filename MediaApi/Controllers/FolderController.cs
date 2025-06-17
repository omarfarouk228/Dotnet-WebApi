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
    public class FolderController(IFolderService folderService) : ControllerBase
    {
        private readonly IFolderService _folderService = folderService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Folder>>> GetAll()
        {
            var folders = await _folderService.GetAllFolders();
            return Ok(folders);
        }

        [HttpGet("children/{id:int}")]
        public async Task<ActionResult<IEnumerable<Folder>>> GetChildren(int id)
        {
            var folders = await _folderService.GetChildrens(id);
            return Ok(folders);
        }

        [HttpGet("hasfile/{id:int}")]
        public async Task<ActionResult<bool>> HasFiles(int id)
        {
            var folders = await _folderService.HasFiles(id);
            return Ok(folders);
        }

        [HttpGet("haschildrens/{id:int}")]
        public async Task<ActionResult<bool>> HasChildrens(int id)
        {
            var folders = await _folderService.HasChildrens(id);
            return Ok(folders);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] FolderCreate folder)
        {
            var id = await _folderService.CreateFolder(folder);
            return Ok(id);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Folder?>> Get(int id)
        {
            var folder = await _folderService.GetFolderById(id);
            return Ok(folder);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] FolderUpdate folder)
        {
            if (id != folder.Id)
            {
                return BadRequest("Id non correspondant");
            }
            var result = await _folderService.UpdateFolder(folder);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _folderService.DeleteFolder(id);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}