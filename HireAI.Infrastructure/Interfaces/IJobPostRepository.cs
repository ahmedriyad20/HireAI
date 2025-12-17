using HireAI.Data.Helpers.DTOs.Applicant.response;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IJobPostRepository : IGenericRepositoryAsync<JobPost> {
                     
        public Task<ICollection<JobPost>?>  GetJobPostForHrAsync(int hrid);

        public Task<ICollection<ApplicantDto>> GetApplicantDtosForJobAsync(int jobId);

        public Task<int> GetTotalApplicationAsncy(int jobId);

        public Task<int> GetTotalApplicationsByJobIdAsync(int jobId);
    }
}
