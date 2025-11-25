using HireAI.Data.Helpers.Enums;
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
        public enApplicationStatus ApplicationStatus { get; set; }
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;
        public string? CVFilePath { get; set; }
        public float? AtsScore { get; set; }

        //Foreign Keys
        public int? HRId { get; set; }
        public int? ApplicantId { get; set; }
        public int? JobId { get; set; }
        public int? ExamId { get; set; }

        //Navigation Property
        public HR? HR { get; set; }
        public Applicant? Applicant { get; set; }
        public JobOpening? AppliedJob { get; set; }
        public Exam? Exam { get; set; }
        public ExamSummary? ExamSummary { get; set; }

        //add this exam status
        public enExamStatus? ExamStatus { get; set; }


    }
}
