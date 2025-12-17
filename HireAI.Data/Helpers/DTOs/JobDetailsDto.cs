using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    /// <summary>
    /// DTO for detailed job information
    /// </summary>
    public class JobDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public enJobStatus JobStatus { get; set; }
        public int? ExamDurationMinutes { get; set; }
        public enExperienceLevel? ExperienceLevel { get; set; }
        public enEmploymentType? EmploymentType { get; set; }
        public string? Location { get; set; }
        public string? SalaryRange { get; set; }
        public int? NumberOfQuestions { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public int? ATSMinimumScore { get; set; }
        public bool AutoSend { get; set; }
        
        // Statistics
        public int TotalApplications { get; set; }
        public int ApplicantsTookExam { get; set; }
        public List<JobApplicationDto> Applications { get; set; } = new();
        public List<TopApplicantDto> TopApplicants { get; set; } = new();
        public List<TopExamTakerDto> TopExamTakers { get; set; } = new();
    }
}
