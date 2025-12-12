using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.DTOs
{
    public class JobOpeningDTO
    {
        public int JobId { get; set; }
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [EnumDataType(typeof(enJobStatus), ErrorMessage = "Invalid job status value.")]
        public enJobStatus JobStatus { get; set; }

        public int? ExamDurationMinutes { get; set; }

        [EnumDataType(typeof(enExperienceLevel), ErrorMessage = "Invalid experience level value.")]
        public enExperienceLevel? ExperienceLevel { get; set; }

        [EnumDataType(typeof(enEmploymentType), ErrorMessage = "Invalid employment type value.")]
        public enEmploymentType? EmploymentType { get; set; }

        public string? Location { get; set; }
        public string? SalaryRange { get; set; }
        public int? NumberOfQuestions { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public int? ATSMinimumScore { get; set; }
    }
}
