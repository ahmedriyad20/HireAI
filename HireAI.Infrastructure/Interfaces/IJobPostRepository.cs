using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IJobPostRepository : IGenericRepositoryAsync<JobPost> {
                     
        public Task<ICollection<JobPost>?>  GetJobPostForHrAsync(int hrid); 
    }
}
