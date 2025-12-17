using HireAI.Data.Helpers.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.DTOs.ApplicantDashboard
{
    public class ApplicationTimelineItemDto
    {
        public string JobTitle { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public DateTime AppliedAt { get; set; } = DateTime.Now;
        public float? AtsScore { get; set; }
        public string ApplicationStatus { get; set; } = default!; // Changed from enApplicationStatus
    }
}
