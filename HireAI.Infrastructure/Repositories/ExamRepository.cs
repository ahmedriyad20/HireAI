using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ExamRepository : GenericRepositoryAsync<Exam>, IExamRepository
    {
        public ExamRepository(HireAIDbContext db) : base(db) { }

        //Get Exam by Applicant Id with Questions and Answers
        public async Task<Exam?> GetExamByApplicanIdAsync(int id)
        {

            return await _dbSet.Include(e=>e.Questions)
                               .ThenInclude(q=> q.Answers)
                               .Where(e => e.ApplicantId == id)
                               .FirstOrDefaultAsync();  
        }

        public async Task<ICollection<Exam>?> GetExamsByApplicantIdAsync(int applicantId, int pageNumber =1, int pageSize  =5)
        {

            return await _dbSet.Include(e => e.Questions)
                               .ThenInclude(q => q.Answers)
                               .Skip(pageSize * (pageNumber - 1))
                               .Take(pageSize)
                               .Where(e => e.ApplicantId == applicantId).ToListAsync();                
        }
    }
}
