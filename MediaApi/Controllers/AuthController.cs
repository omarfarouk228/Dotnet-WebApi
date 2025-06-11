using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public async Task<AuthResult> ForgotPasswordAsync(string email)
        {
            // Générer un token temporaire sécurisé
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await _emailService.SendEmailAsync(email, "Réinitialisation du mot de passe", $"Cliquez ici pour réinitialiser votre mot de passe : <a href=\"https://tonsite/reset-password?token={token}\">Réinitialiser</a>");

            // (Optionnel) Envoi du token par email
            Console.WriteLine($"Lien de réinitialisation: https://votresite/reset-password?token={token}");

            return new AuthResult { IsSuccess = true, Message = "Lien de réinitialisation envoyé." };
        }



    }
}