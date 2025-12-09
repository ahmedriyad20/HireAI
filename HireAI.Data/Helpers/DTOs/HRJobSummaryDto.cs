using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    /// <summary>
    /// DTO for HR's job list summary
    /// </summary>
    public class HRJobSummaryDto
    {
        public int HRId { get; set; }
        public int JobId { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string JobStatus { get; set; }
        public int TotalApplications { get; set; }
        public int ApplicantsTookExam { get; set; }
        public float? AvgATSScore { get; set; }
        public float? AvgExamScore { get; set; }
    }
}
