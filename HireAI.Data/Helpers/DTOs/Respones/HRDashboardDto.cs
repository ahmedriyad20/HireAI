using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto
{
    public class HRDashboardDto
    {
        public int TotalApplicants { get; set; }
        public int TotalExamTaken { get; set; }
        public int TotalTopApplicants { get; set; }

        public int ATSPassedRate{ get; set; }
        public Dictionary<int ,int> MonthlyApplicants { get; set; } = new Dictionary<int ,int>(); //Key: Month, Value: Number of Applicants
        public Dictionary<int , int> ATSPassedRateMonthly { get; set; } = new Dictionary<int , int>(); //Key: Month, Value: ATS Passed Rate

        public Dictionary<string, float> ExamScoreDistribution { get; set; } = new Dictionary<string, float>(); //Key: "0-20", "20-40", etc., Value: Percentage

        public List<RecentApplicationDto> RecentApplications { get; set; } = new List<RecentApplicationDto>();
        public List<ActiveJobPosting> ActiveJobPostings { get; set; } = new List<ActiveJobPosting>();
    }
}
