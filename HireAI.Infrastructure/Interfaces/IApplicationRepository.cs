using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.Intrefaces
{
    public interface IApplicationRepository : IGenericRepositoryAsync<Application> {

        public  Task<int?> GetAtsPassingScore(int jobId);
        public Task<float?> GetAvgExamPassingScore(int jobId);

    }
}
