using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public  class Application
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        
        public JobOpening Job { get; set; } = null!;

        public int ApplicantId { get; set; }
        
        public Applicant Applicant { get; set; } = null!;

        public int ApplicationStatus { get; set; }
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;
        public string? CVFilePath { get; set; }
        public float? ScoreATS { get; set; }

        // Navigation
        public ICollection<Exam>? Tests { get; set; }
        public ICollection<TestAttempt>? TestAttempts { get; set; }
    }
}
