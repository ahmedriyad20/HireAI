using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IJobOpeningRepository : IGenericRepositoryAsync<JobOpening> { 
        public Task<ICollection<JobOpening>?>  GetJobOpeningForHrAsync(int hrid); 
    }
}
