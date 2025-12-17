using System;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.DTOs.ApplicantDashboard
{
    public class ApplicantSkillImprovementDto
    {
        public string SkillName { get; set; } = default!;
        public DateTime Month { get; set; }
        public float? SkillRating { get; set; }
        public float? ImprovementPercentage { get; set; }
        public string? Notes { get; set; }
    }
}
