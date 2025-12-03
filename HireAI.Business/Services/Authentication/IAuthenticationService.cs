using HireAI.Data.Helpers.DTOs.Authentication;

namespace HireAI.Business.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> RegisterApplicantAsync(RegisterApplicantDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync(string userId);
    }
}