using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Applicant.response
{
    public class ApplicantDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public float? AtsScore { get; set; }
        public float? ExamScore { get; set; }
        public string? Status { get; set; } = string.Empty;
    }
}
