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
    public class GroupController(IGroupService groupService) : ControllerBase
    {
        private readonly IGroupService _groupService = groupService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetAll([FromQuery] PaginationParams query)
        {
            var groups = await _groupService.GetAllGroups(query);
            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] Group group)
        {
            var id = await _groupService.CreateGroup(group);
            return Ok(id);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Group?>> Get(int id)
        {
            var group = await _groupService.GetGroupWithMedias(id);
            return Ok(group);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] Group group)
        {
            if (id != group.Id)
            {
                return BadRequest("Id non correspondant");
            }
            var result = await _groupService.UpdateGroup(group);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            var result = await _groupService.DeleteGroup(id);

            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}