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
    public class AuthController(IAuthService authService, IEmailService emailService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IEmailService _emailService = emailService;

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

        [HttpPost("forgot-password")]
        public async Task<ActionResult<AuthResult>> ForgotPassword([FromBody] ForgotDto forgotDto)
        {
            var response = await _authService.ForgotPasswordAsync(forgotDto.Email);

            // Envoyer le mail
            if (response.IsSuccess)
            {
                var message = $"Voici le lien de réinitialisation du mot de passe : http://localhost:5207/reset-password/{response.Token}";
                await _emailService.SendEmailAsync(forgotDto.Email, "Réinitialisation du mot de passe", message);
            }

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<AuthResult>> ResetPassword([FromBody] ResetDto resetDto)
        {
            var response = await _authService.ResetPasswordAsync(resetDto.Token, resetDto.Password);

            return Ok(response);
        }

    }
}