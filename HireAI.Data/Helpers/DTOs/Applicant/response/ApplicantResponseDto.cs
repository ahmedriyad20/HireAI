using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Applicant.Response
{
    /// <summary>
    /// DTO used to return Applicant data in API responses.
    /// Contains all publicly accessible applicant information.
    /// </summary>
    public class ApplicantResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public DateOnly DateOnly { get; set; }

        public string? Phone { get; set; }

        public string? Bio { get; set; }

        public string? Title { get; set; }

        public string ResumeUrl { get; set; } = default!;

        public enSkillLevel? SkillLevel { get; set; }

        public bool IsActive { get; set; }

        public bool IsPremium { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }

        public int? CVId { get; set; }
    }
}