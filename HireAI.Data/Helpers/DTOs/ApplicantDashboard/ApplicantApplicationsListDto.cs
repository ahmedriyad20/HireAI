using HireAI.Data.Helpers.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.DTOs.ApplicantDashboard
{
    public class ApplicantApplicationsListDto
    {
        public int ApplicationId { get; set; }
        public string JobTitle { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public string CompanyLocation { get; set; } = default!;
        public DateTime AppliedAt { get; set; } = DateTime.Now;
        public float? AtsScore { get; set; }
        public string ApplicationStatus { get; set; } = default!; // Changed from enApplicationStatus to string
        public string? JobType { get; set; } // Added JobType property (FullTime, PartTime, Internship, FreeLance)
        public int JobId { get; set; }
        public enExamEvaluationStatus ExamEvaluationStatus { get; set; } = enExamEvaluationStatus.Pending;
    }
}
