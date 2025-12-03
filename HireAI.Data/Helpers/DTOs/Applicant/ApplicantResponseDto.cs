using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;

namespace HireAI.Data.Helpers.DTOs.Applicant
{
    public class ApplicantResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ResumeUrl { get; set; } = default!;
        public enSkillLevel? SkillLevel { get; set; } = enSkillLevel.Beginner;

        public ICollection<ApplicantSkillDto> ApplicantSkills { get; set; } = new HashSet<ApplicantSkillDto>();
    }
}
