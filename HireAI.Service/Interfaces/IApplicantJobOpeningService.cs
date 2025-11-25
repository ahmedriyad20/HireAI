using HireAI.Data.DTOs;

namespace HireAI.Service.Interfaces
{
    public interface IApplicantJobOpeningService
    {
        public Task<List<JobOpeningDTO>> GetAllJobOpeningAsync();
    }
}
