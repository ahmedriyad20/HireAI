namespace HireAI.Data.Helpers.DTOs.Authentication
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}