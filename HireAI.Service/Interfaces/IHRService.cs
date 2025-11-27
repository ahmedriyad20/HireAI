using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Data.Helpers.DTOs.Respones;
using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IHRService
    {
        Task<HRDashboardDto> GetDashboardAsync(int hrId);
        Task<HRResponseDto> GetHRAsync(int hrId);

        // Individual dashboard metric endpoints
        Task<int> GetTotalApplicantsAsync(int hrId);
        Task<int> GetTotalExamTakenAsync(int hrId);
        Task<int> GetTotalTopCandidatesAsync(int hrId);
        Task<Dictionary<int, int>> GetMonthlyNumberOfApplicationsAsync(int hrId);
        Task<Dictionary<int, int>> GetMonthlyOfTotalATSPassedAsync(int hrId);
        Task<List<RecentApplicationDto>> GetRecentApplicantsAsync(int hrId, int take = 5);
        Task<List<ActiveJopPosting>> GetActiveJobPostingsAsync(int hrId);
    }
}