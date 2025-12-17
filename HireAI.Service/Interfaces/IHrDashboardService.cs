using HireAI.Data.Helpers.DTOs.Respones;
using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;

namespace HireAI.Service.Interfaces
{
    public interface IHrDashboardService
    {
        public Task<HRDashboardDto> GetDashboardAsync(int hrId);
        
        public Task<int> GetTotalApplicantsAsync(int hrId);
        
        public Task<int> GetTotalExamTakenAsync(int hrId);
        
        public Task<int> GetTotalTopCandidatesAsync(int hrId);
        
        public Task<Dictionary<int, int>> GetMonthlyNumberOfApplicationsAsync(int hrId);
        
        public Task<Dictionary<int, int>> GetMonthlyOfTotalATSPassedAsync(int hrId);
        
        public Task<List<RecentApplicationDto>> GetRecentApplicantsAsync(int hrId, int take = 5);
        
        public Task<List<ActiveJobPosting>> GetActiveJobPostingsAsync(int hrId);
    }
}
