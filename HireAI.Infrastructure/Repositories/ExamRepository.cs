using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ExamRepository : GenericRepositoryAsync<Exam>, IExamRepository
    {
        public ExamRepository(HireAIDbContext db) : base(db) { }

       

        //Get Exam by Applicant Id with Questions and Answers
        public async Task<Exam?> GetExamByApplicanIdAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Questions)
                .Include(e => e.Applications)
                .Where(e => e.Applications.Any(a => a.ApplicantId == id))
                .FirstOrDefaultAsync();
        }

        // Get Exams by Applicant Id with pagination
        public async Task<ICollection<Exam>?> GetExamsByApplicantIdAsync(int applicantId, int pageNumber = 1, int pageSize = 5)
        {
            return await _dbSet
                .Include(e => e.Questions)
                .Include(e => e.Applications)
                .Where(e => e.Applications.Any(a => a.ApplicantId == applicantId))
                .OrderByDescending(e => e.CreatedAt)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task CreateExamAsncy(Exam exam)
        {
            await _dbSet.AddAsync(exam);
            await _context.SaveChangesAsync();
            
        }
    }
}
