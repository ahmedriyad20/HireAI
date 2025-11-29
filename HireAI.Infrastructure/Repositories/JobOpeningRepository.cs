using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class JobOpeningRepository : GenericRepositoryAsync<JobOpening>, IJobOpeningRepository
    {
        public JobOpeningRepository(HireAIDbContext db) : base(db) { }

        public async Task<ICollection<JobOpening>?> GetJobOpeningForHrAsync(int hrid)
        {
            return await _dbSet
                           .Where(jo => jo.HRId == hrid)
                           .ToListAsync();
        }
    }
}
