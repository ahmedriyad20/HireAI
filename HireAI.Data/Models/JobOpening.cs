using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class JobOpening
    {

        public int Id { get; set; }

        public int HRId { get; set; }
        public HR HR { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public JobStatus Status { get; set; } = JobStatus.Open;
        public int? ExamDurationMinutes { get; set; } // in minutes
        public int? ExperienceLevel { get; set; }
        public int? EmploymentType { get; set; }
        public string? Location { get; set; }
        public string? SalaryRange { get; set; }
        public int? NumberOfQuestions { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public int? ATSMinimumScore { get; set; }
        public bool AutoSend { get; set; } = false;

        // Navigation
        public ICollection<JobSkill>? JobSkills { get; set; }
        public ICollection<Application>? Applications { get; set; }
        public ICollection<TestAttempt>? TestAttempts { get; set; }
    }
}
