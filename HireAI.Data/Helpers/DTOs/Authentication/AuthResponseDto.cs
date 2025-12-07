namespace HireAI.Data.Helpers.DTOs.Authentication
{
    public class AuthResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public string? RefreshToken { get; set; }
        public string? Message { get; set; }
        public string? IdentityUserId { get; set; } // ASP.NET Identity User ID
        public int? UserId { get; set; } // ApplicantId or HRId
        public string? UserRole { get; set; } // "Applicant" or "HR"
        public List<string>? Errors { get; set; } = new();
    }
}