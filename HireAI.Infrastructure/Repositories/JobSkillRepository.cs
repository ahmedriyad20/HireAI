using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class JobSkillRepository : GenericRepositoryAsync<JobSkill>, IJobSkillRepository
    {
        public JobSkillRepository(HireAIDbContext db) : base(db) { }

        public async Task<ICollection<JobSkill>> GetSkillsByJobIdAsync(int jobId)
        {
            return await GetAll()
                .Where(js => js.JobId == jobId)
                .ToListAsync();
        }
    }
}
