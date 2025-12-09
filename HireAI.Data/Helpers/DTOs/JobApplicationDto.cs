using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    /// <summary>
    /// DTO for job application with applicant details
    /// </summary>
    public class JobApplicationDto
    {
        public int ApplicationId { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantPhone { get; set; }
        public string CVKey { get; set; }
        public DateTime DateApplied { get; set; }
        public float? ATSScore { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public enExamStatus ExamStatus { get; set; }
    }
}
