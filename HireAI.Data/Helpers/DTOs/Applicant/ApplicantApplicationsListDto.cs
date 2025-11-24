using HireAI.Data.Helpers.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Service.DTOs
{
    public class ApplicantApplicationsListDto
    {
        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Job title must be between 2 and 150 characters.")]
        public string JobTitle { get; set; } = default!;

        [StringLength(150, ErrorMessage = "Company name must not exceed 150 characters.")]
        public string CompanyName { get; set; } = default!;

        [StringLength(150, ErrorMessage = "Company location must not exceed 150 characters.")]
        public string CompanyLocation { get; set; } = default!;

        [Required(ErrorMessage = "Applied date is required.")]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        [Range(0, 100, ErrorMessage = "ATS score must be between 0 and 100.")]
        public float? AtsScore { get; set; }

        [Required(ErrorMessage = "Application status is required.")]
        [EnumDataType(typeof(enApplicationStatus), ErrorMessage = "Invalid application status.")]
        public enApplicationStatus ApplicationStatus { get; set; }
    }
}
