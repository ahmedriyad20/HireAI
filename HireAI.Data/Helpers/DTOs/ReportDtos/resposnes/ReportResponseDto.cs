using HireAI.Data.Helpers.DTOs.Applicant.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.ReportDtos.resposnes
{
    public class ReportDto
    {
        public string JobTitle { get; set; } = string.Empty;
        public int TotalApplicants { get; set; }
        public float AtsPassPercent { get; set; }
        public List<ApplicantDto> Applicants { get; set; } = new List<ApplicantDto>();
    }
}
