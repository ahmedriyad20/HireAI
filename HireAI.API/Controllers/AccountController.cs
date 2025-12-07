using HireAI.Data.Helpers.DTOs.Authentication;
using Microsoft.AspNetCore.Mvc;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterApplicant")]
        [AllowAnonymous] // Public - anyone can register
        public async Task<IActionResult> RegisterApplicant([FromBody] RegisterApplicantDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            var response = await _authService.RegisterApplicantAsync(registerDto);
            
            return response.IsAuthenticated ? Ok(response) : BadRequest(response);
        }

        [HttpPost("RegisterHR")]
        [AllowAnonymous] // Public - anyone can register
        public async Task<IActionResult> RegisterHR([FromBody] RegisterHrDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            var response = await _authService.RegisterHRAsync(registerDto);

            return response.IsAuthenticated ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });

            var response = await _authService.LoginAsync(loginDto);
            
            return response.IsAuthenticated ? Ok(response) : Unauthorized(response);
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var response = await _authService.RefreshTokenAsync(refreshTokenDto.AccessToken, refreshTokenDto.RefreshToken);
            
            return response.IsAuthenticated ? Ok(response) : Unauthorized(response);
        }

        [HttpPost("Logout")]
        [Authorize] // Authenticated users only (any role)
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated" });

            var success = await _authService.RevokeTokenAsync(userId);
            
            return success ? Ok(new { message = "Logout successful" }) : BadRequest(new { message = "Logout failed" });
        }
    }
}