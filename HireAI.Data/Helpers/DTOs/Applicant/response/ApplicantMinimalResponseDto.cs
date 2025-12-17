using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Applicant.Response
{
    /// <summary>
    /// DTO used to return minimal Applicant data in list views or nested objects.
    /// Contains only essential applicant information.
    /// </summary>
    public class ApplicantMinimalResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string? Phone { get; set; }

        public string? Title { get; set; }

        public string ResumeUrl { get; set; } = default!;

        public enSkillLevel? SkillLevel { get; set; }
    }
}