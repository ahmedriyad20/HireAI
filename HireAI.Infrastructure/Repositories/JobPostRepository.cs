using HireAI.Data.Helpers.DTOs.Applicant.response;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HireAI.Infrastructure.Repositories
{
    public class JobPostRepository : GenericRepositoryAsync<JobPost>, IJobPostRepository
    {
        private readonly IApplicationRepository _applicationRepository;

        public JobPostRepository(HireAIDbContext db ,IApplicationRepository applicationRepository) : base(db)
        { 
            _applicationRepository = applicationRepository;
        }

        //public override async Task<IQueryable<JobPost>> GetAll()
        //{
        //    return await _dbSet
        //                    .Include(jo => jo.ExamEvaluations)
        //}

        public async Task<ICollection<JobPost>?> GetJobPostForHrAsync(int hrid)
        {
            return await _dbSet
                           .Where(jo => jo.HRId == hrid)
                           .Include(jo => jo.Applications)
                           .Include(jo => jo.ExamEvaluations)
                           .Include(jp => jp.JobSkills)
                           .ThenInclude(js => js.Skill)
                           .ToListAsync();
        }

        public override Task<JobPost?> GetByIdAsync(int id)
        {
            return _dbSet
                    .Include(jo => jo.Applications)
                    .Include(jo => jo.ExamEvaluations)
                    .Include(jp => jp.JobSkills)
                    .ThenInclude(js => js.Skill)
                    .FirstOrDefaultAsync(jp => jp.Id == id);

        }
       
          public async Task<ICollection<ApplicantDto>> GetApplicantDtosForJobAsync(int jobId)
        {
            var result = await _context.Applications
                .AsNoTracking()
                .Where(ap => ap.JobId == jobId)
                .Select(ap => new ApplicantDto
                {
                    // Applicant navigation used only for projecting fields; EF will translate to JOIN
                    Name = ap.Applicant != null ? ap.Applicant.FullName : null,
                    Email = ap.Applicant != null ? ap.Applicant.Email : null,
                    AtsScore = ap.AtsScore,
                    // Null-safe projection of ExamEvaluation.TotalScore
                    ExamScore = ap.ExamEvaluation != null ? (float?)ap.ExamEvaluation.ExamTotalScore: null,
                }).OrderByDescending(a => a.AtsScore)
                .ToListAsync();

            return result;
        }

       

        public async Task<int> GetTotalApplicationAsncy(int jobId)
        {
                      return await _dbSet
                          .Where(j=> j.Id==jobId)
                          .Join(
                              _applicationRepository.GetAll(), 
                               job =>job.Id,
                               application => application.JobId,
                               (job, application) => application    
                               ).CountAsync();

            
        }

        public async Task<int> GetTotalApplicationsByJobIdAsync(int jobId)
        {
            return await _context.Applications
                .Where(app => app.JobId == jobId)
                .CountAsync();
        }

    }
}
