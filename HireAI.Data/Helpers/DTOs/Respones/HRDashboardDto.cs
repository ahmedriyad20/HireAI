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
        public int TotalTopCandidates { get; set; }

        public int ATSPassedRate{ get; set; }
        public Dictionary<int ,int> MonthlyApplicants { get; set; } = new Dictionary<int ,int>(); //Key: Month, Value: Number of Applicants
        public Dictionary<int , int> ATSPassedRateMonthly { get; set; } = new Dictionary<int , int>(); //Key: Month, Value: ATS Passed Rate

        public Dictionary<(int,int), float> ExamScoreDistribution { get; set; } = new Dictionary<(int,int), float>(); //Performance breakdown by score range

        public List<RecentApplicationDto> RecentApplications { get; set; } = new List<RecentApplicationDto>();
        public List<ActiveJopPosting> ActiveJopPostings { get; set; } = new List<ActiveJopPosting>();
    }
}
