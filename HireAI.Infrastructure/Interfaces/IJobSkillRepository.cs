using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.Intrefaces
{
    public interface IJobSkillRepository : IGenericRepositoryAsync<JobSkill> {  
      public Task<ICollection<JobSkill>> GetSkillsByJobIdAsync(int jobId);
    }
}
