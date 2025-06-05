using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using MediaApi.dto;
using MediaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterDto registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto.Username, registerDto.Email, registerDto.Password);
            return Ok(response);
        }

    }
}