using HireAI.Data.Helpers.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> RegisterApplicantAsync(RegisterApplicantDto registerDto);
        Task<AuthResponseDto> RegisterHRAsync(RegisterHrDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, ClaimsPrincipal user);
        Task<AuthResponseDto> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync(string userId);
        Task<AuthResponseDto> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<AuthResponseDto> ChangeEmailAsync(string userId, string newEmail);
        Task<AuthResponseDto> DeleteAccountAsync(string userId, string password);
    }
}
