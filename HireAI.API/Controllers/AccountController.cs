using HireAI.Data.Helpers.DTOs.Authentication;
using HireAI.Service.Interfaces;
using HireAI.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly Service.Interfaces.IAuthorizationService _authorizationService;

        public AccountController(IAuthenticationService authService, Service.Interfaces.IAuthorizationService authorizationService)
        {
            _authService = authService;
            _authorizationService = authorizationService;
        }

        [HttpPost("RegisterApplicant")]
        [AllowAnonymous] // Public - anyone can register
        public async Task<IActionResult> RegisterApplicant([FromForm] RegisterApplicantDto registerDto)
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

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var result = await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

            return result.IsAuthenticated ? Ok(result) : BadRequest(result);
        }

        [HttpPut("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var result = await _authService.ChangeEmailAsync(userId, dto.NewEmail);

            return result.IsAuthenticated ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var result = await _authService.DeleteAccountAsync(userId, dto.Password);

            return result.IsAuthenticated == false && result.Message == "Account deleted successfully"
                ? Ok(result)
                : BadRequest(result);
        }
    }
}