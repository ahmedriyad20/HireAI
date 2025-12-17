using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ApplicationRepository : GenericRepositoryAsync<Application>, IApplicationRepository
    {
        public ApplicationRepository(HireAIDbContext db) : base(db) { }

        public async Task<int?> GetAtsPassingScore(int jobId)
        {
           return await _dbSet.Where(app => app.JobId == jobId & app.AtsScore>70).CountAsync();
        }

        public async Task<float?> GetAvgExamPassingScore(int jobId)
        {
            var query = _dbSet
                .Where(app => app.JobId == jobId && app.ExamEvaluation != null)
                .Select(app => app.ExamEvaluation.ExamTotalScore)
                .Where(score => score > 70);

            var list = await query.ToListAsync();

            if (!list.Any())
                return 0;

            return list.Average();
        }
    }
}






