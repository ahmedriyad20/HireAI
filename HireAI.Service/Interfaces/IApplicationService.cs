using HireAI.Data.Helpers.DTOs.Application;

namespace HireAI.Service.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicationResponseDto>> GetAllApplicationsAsync();
        Task<ApplicationResponseDto?> GetApplicationByIdAsync(int applicationId);
        Task<IEnumerable<ApplicationResponseDto>> GetApplicationsByApplicantIdAsync(int applicantId);
        Task<IEnumerable<ApplicationResponseDto>> GetApplicationsByJobIdAsync(int jobId);
        Task<IEnumerable<ApplicationResponseDto>> GetApplicationsByHRIdAsync(int hrId);
        Task<ApplicationResponseDto> CreateApplicationAsync(CreateApplicationDto createDto);
        Task<ApplicationResponseDto> UpdateApplicationAsync(UpdateApplicationDto updateDto);
        Task<bool> DeleteApplicationAsync(int applicationId);
    }
}