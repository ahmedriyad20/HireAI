using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.DTOs
{
    public class JobOpeningDTO
    {
        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Job title must be between 3 and 200 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 150 characters.")]
        public string CompanyName { get; set; } = default!;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Job status is required.")]
        [EnumDataType(typeof(enJobStatus), ErrorMessage = "Invalid job status value.")]
        public enJobStatus JobStatus { get; set; }

        [Range(1, 480, ErrorMessage = "Exam duration must be between 1 and 480 minutes.")]
        public int? ExamDurationMinutes { get; set; }

        [EnumDataType(typeof(enExperienceLevel), ErrorMessage = "Invalid experience level value.")]
        public enExperienceLevel? ExperienceLevel { get; set; }

        [EnumDataType(typeof(enEmploymentType), ErrorMessage = "Invalid employment type value.")]
        public enEmploymentType? EmploymentType { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string? Location { get; set; }

        [StringLength(50, ErrorMessage = "Salary range cannot exceed 50 characters.")]
        [RegularExpression(@"^\d+k?\s*-\s*\d+k?$|^\d+k?$", ErrorMessage = "Salary range format should be like '50k-100k' or '50k'.")]
        public string? SalaryRange { get; set; }

        [Range(1, 500, ErrorMessage = "Number of questions must be between 1 and 500.")]
        public int? NumberOfQuestions { get; set; }

        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(JobOpeningDTOValidation), nameof(JobOpeningDTOValidation.ValidateApplicationDeadline))]
        public DateTime? ApplicationDeadline { get; set; }

        [Range(0, 100, ErrorMessage = "ATS minimum score must be between 0 and 100.")]
        public int? ATSMinimumScore { get; set; }
    }

    // Custom validation for ApplicationDeadline
    public static class JobOpeningDTOValidation
    {
        public static ValidationResult? ValidateApplicationDeadline(DateTime? deadline, ValidationContext context)
        {
            if (deadline.HasValue && deadline.Value <= DateTime.UtcNow)
            {
                return new ValidationResult("Application deadline must be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
