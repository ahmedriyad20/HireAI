using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class JobPostRepository : GenericRepositoryAsync<JobPost>, IJobPostRepository
    {
        public JobPostRepository(HireAIDbContext db) : base(db) { }

        public async Task<ICollection<JobPost>?> GetJobPostForHrAsync(int hrid)
        {
            return await _dbSet
                           .Where(jo => jo.HRId == hrid)
                           .ToListAsync();
        }
    }
}
