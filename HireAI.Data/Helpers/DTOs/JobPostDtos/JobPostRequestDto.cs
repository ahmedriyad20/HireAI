using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.JobOpening.Request
{
    /// <summary>
    /// DTO used to create a 
    /// .
    /// Contains the fields relevant for creation and simple validation attributes.
    /// </summary>
    public class JobPostRequestDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        // CompanyName is optional if derived from the HR, include it for flexibility
        [MaxLength(200)]
        public string? CompanyName { get; set; }

        [MaxLength(3000)]
        public string? Description { get; set; }

        public enJobStatus? JobStatus { get; set; } = enJobStatus.Active;

        // in minutes
        [Range(10, 40, ErrorMessage = "Exam duration must be between 10 and 40 minutes.")]
        public int? ExamDurationMinutes { get; set; }

        public enExperienceLevel? ExperienceLevel { get; set; }

        public enEmploymentType? EmploymentType { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? SalaryRange { get; set; }

        [Range(10, 40, ErrorMessage = "Exam fQuestions must be between 10 and 60 minutes.")]
        public int? NumberOfQuestions { get; set; }

        public DateTime? ApplicationDeadline { get; set; }

        public int? ATSMinimumScore { get; set; }

        public bool AutoSend { get; set; } = false;

        // HRId typically comes from the authenticated user; keep optional to allow server assignment
        public int? HRId { get; set; }

        // Optional list of skill ids to associate with the job opening
        public IEnumerable<int>? SkillIds { get; set; }
    }
}
