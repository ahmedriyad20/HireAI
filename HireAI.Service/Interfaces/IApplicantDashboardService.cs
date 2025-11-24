using HireAI.Service.DTOs;

namespace HireAI.Service.Interfaces
{
    public interface IApplicantDashboardService
    {
        Task<int> GetActiveApplicationsNumberPerApplicantAsync(int applicantId);
        Task<int> GetMockExamsTakenNumberPerApplicantAsync(int applicantId);
        Task<double> GetAverageExamsTakenScorePerApplicantAsync(int applicantId);
        Task<string> GetSkillLevelPerApplicantAsync(int applicantId);
        Task<IEnumerable<ApplicationTimelineItemDto>> GetApplicationTimelinePerApplicantAsync(int applicantId);
    }
}
