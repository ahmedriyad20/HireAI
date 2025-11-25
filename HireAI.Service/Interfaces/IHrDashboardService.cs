using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;

namespace HireAI.Service.Interfaces
{
    public interface IHrDashboardService
    {

        public Task<HRDashboardDto> GetDashboardAsync(int hrId);
    }
}
