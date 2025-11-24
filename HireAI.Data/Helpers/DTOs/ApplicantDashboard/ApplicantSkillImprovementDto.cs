using System;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.DTOs.ApplicantDashboard
{
    public class ApplicantSkillImprovementDto
    {
        [Required(ErrorMessage = "Skill name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Skill name must be between 2 and 100 characters.")]
        public string SkillName { get; set; } = default!;

        [Required(ErrorMessage = "Month and year are required.")]
        public DateTime Month { get; set; }

        [Range(0, 100, ErrorMessage = "Skill rating must be between 0 and 100.")]
        public float? SkillRating { get; set; }

        [Range(0, 100, ErrorMessage = "Improvement percentage must be between 0 and 100.")]
        public float? ImprovementPercentage { get; set; }

        [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}
