using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Data.Helpers.DTOs.ApplicantSkill;
using HireAI.Data.Helpers.DTOs.ApplicantSkill.Response;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Helpers.Enums;

namespace HireAI.Data.Helpers.DTOs.Applicant.Response
{
    /// <summary>
    /// DTO used to return detailed Applicant data including related entities.
    /// Contains all applicant information plus related skills, applications, and exams.
    /// </summary>
    public class ApplicantDetailedResponseDto
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

        public enRole Role { get; set; }

        public IEnumerable<ApplicantSkillMinimalResponseDto>? ApplicantSkills { get; set; } = new HashSet<ApplicantSkillMinimalResponseDto>();

        public IEnumerable<int>? ApplicationIds { get; set; } = new HashSet<int>();

        public IEnumerable<int>? ExamIds { get; set; } = new HashSet<int>();
    }
}