using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class JobPost
    {
        public int Id { get; set; }
  
        public string Title { get; set; } = null!;
        public string CompanyName { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public enJobStatus JobStatus { get; set; } 

        public int? ExamDurationMinutes { get; set; } // in minutes
        public enExperienceLevel? ExperienceLevel { get; set; }
        public enEmploymentType? EmploymentType { get; set; }
        public string? Location { get; set; }
        public string? SalaryRange { get; set; }
        public int? NumberOfQuestions { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public int? ATSMinimumScore { get; set; }
        public bool AutoSend { get; set; } = false;

        //Foreign Keys
        public int HRId { get; set; }


        // Navigation Property
        public HR? HR { get; set; }
        public ICollection<JobSkill> JobSkills { get; set; } = new HashSet<JobSkill>();
        public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public ICollection<ExamEvaluation> ExamEvaluations { get; set; } = new HashSet<ExamEvaluation>();
        public ICollection<Applicant> Applicants { get; set; } = new HashSet<Applicant>();

    }
}
